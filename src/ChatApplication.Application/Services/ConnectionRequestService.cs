using ChatApplication.Application.Constants;
using ChatApplication.Application.Services.Common;
using ChatApplication.Application.ViewModels.Authentication;
using ChatApplication.Application.ViewModels.ConnectionRequest;
using ChatApplication.Domain.Common;
using ChatApplication.Domain.Entities;
using ChatApplication.Domain.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace ChatApplication.Application.Services;

public interface IConnectionRequestService : IServiceAsync<ConnectionRequest>
{
    Task<Result> ShowConnectionRequestAsync(int memberId, CancellationToken cancellationToken = new());
    Task<Result> SendConnectionRequestAsync(int memberId, SendConnectionRequestDto parameter,
        CancellationToken cancellationToken = new());

    Task<Result> AcceptConnectionRequestAsync(int memberId, AcceptConnectionRequestDto parameter,
        CancellationToken cancellationToken = new());
}
public class ConnectionRequestService : ServiceAsync<ConnectionRequest>, IConnectionRequestService
{
    private readonly IUnitOfWorkAsync _unitOfWork;
    private readonly IGroupService _groupService;

    public ConnectionRequestService(IRepositoryAsync<ConnectionRequest> repository, IUnitOfWorkAsync unitOfWork, IGroupService groupService) : base(repository)
    {
        _unitOfWork = unitOfWork;
        _groupService = groupService;
    }

    public async Task<Result> ShowConnectionRequestAsync(int memberId, CancellationToken cancellationToken = new())
    {
        var group = await _groupService.FirstOrDefaultAsync(x => x.Administrator == memberId, cancellationToken);

        if (group == null)
            return Result.WithException(new FailError(Statement.Failure));

        var connectionRequests = await Repository.Queryable(false)
            .Where(x => x.TargetGroupId == group.Id && !x.Accepted)
            .OrderByDescending(x => x.Date)
            .ToListAsync(cancellationToken);

        var values = connectionRequests.Select(x => new
        {
            ConnectionRequestId = x.Id,
            GroupId = x.SourceGroupId,
            Sent = x.Date.ToEpochTimeSpan().TotalSeconds
        });

        return !values.Any() ? Result.WithException(new FailError(Statement.Failure)) : Result.WithSuccess(values);
    }

    public async Task<Result> SendConnectionRequestAsync(int memberId, SendConnectionRequestDto parameter,
        CancellationToken cancellationToken = new())
    {
        var group = await _groupService.FirstOrDefaultAsync(x => x.Administrator == memberId, cancellationToken);

        if (group == null)
            return Result.WithException(new FailError(Statement.Failure));

        if (await Repository.ExistsAsync(x => x.SourceGroupId == group.Id && x.TargetGroupId == parameter.GroupId,
                cancellationToken))
            Result.WithSuccess(new { Message = "Successful" });

        var connectionRequest = new ConnectionRequest()
        {
            SourceGroupId = group.Id,
            TargetGroupId = parameter.GroupId,
            Date = DateTime.UtcNow
        };

        await Repository.AddAsync(connectionRequest, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.WithSuccess(new { Message = "Successful" });
    }

    public async Task<Result> AcceptConnectionRequestAsync(int memberId, AcceptConnectionRequestDto parameter,
        CancellationToken cancellationToken = new())
    {
        var group = await _groupService.FirstOrDefaultAsync(x => x.Administrator == memberId, cancellationToken);

        if (group == null)
            return Result.WithException(new FailError(Statement.Failure));

        var connectionRequest =
            await Repository.FirstOrDefaultAsync(
                x => x.TargetGroupId == group.Id && x.SourceGroupId == parameter.GroupId && !x.Accepted,
                cancellationToken);

        if (connectionRequest == null)
            return Result.WithException(new FailError(Statement.Failure));

        connectionRequest.Accepted = true;

        Repository.Attach(connectionRequest);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.WithSuccess(new { Message = "Successful" });
    }
}