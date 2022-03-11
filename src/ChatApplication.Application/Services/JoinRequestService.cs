using System.Linq;
using ChatApplication.Application.Services.Common;
using ChatApplication.Application.ViewModels.Authentication;
using ChatApplication.Application.ViewModels.JoinRequest;
using ChatApplication.Domain.Common;
using ChatApplication.Domain.Entities;
using ChatApplication.Domain.Enums;
using ChatApplication.Domain.Repositories;
using ChatApplication.Domain.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace ChatApplication.Application.Services;

public interface IJoinRequestService : IServiceAsync<JoinRequest>
{
    Task<Result> ShowMyJoinRequestAsync(int memberId, CancellationToken cancellationToken = new());

    Task<Result> SendJoinRequestAsync(int memberId, SendJoinRequestDto parameter,
        CancellationToken cancellationToken = new());

    Task<Result> ShowJoinRequestToGroupsAsync(int memberId, CancellationToken cancellationToken = new());

    Task<Result> AcceptJoinRequestAsync(int memberId, AcceptJoinRequestDto parameter, CancellationToken cancellationToken = new());
}

public class JoinRequestService : ServiceAsync<JoinRequest>, IJoinRequestService
{
    private readonly IUnitOfWorkAsync _unitOfWork;
    private readonly IGroupRepository _groupRepository;
    private readonly IGroupMemberRepository _groupMemberRepository;

    public JoinRequestService(IRepositoryAsync<JoinRequest> repository, IUnitOfWorkAsync unitOfWork, IGroupRepository groupRepository, IGroupMemberRepository groupMemberRepository) : base(repository)
    {
        _unitOfWork = unitOfWork;
        _groupRepository = groupRepository;
        _groupMemberRepository = groupMemberRepository;
    }

    public async Task<Result> ShowMyJoinRequestAsync(int memberId, CancellationToken cancellationToken = new())
    {
        var joinRequests = await Repository.Queryable(false)
            .Where(x => x.MemberId == memberId)
            .ToListAsync(cancellationToken);

        var values = joinRequests.Select(x => new
        {
            x.Id,
            x.GroupId,
            UserId = x.MemberId,
            Date = x.Date.ToEpochTimeSpan().TotalSeconds
        });

        return Result.WithSuccess(values);
    }

    public async Task<Result> SendJoinRequestAsync(int memberId, SendJoinRequestDto parameter,
        CancellationToken cancellationToken = new())
    {
        var group = await _groupRepository.FirstOrDefaultAsync(x => x.Id == parameter.GroupId, cancellationToken);
        if (group == null || group.Administrator == memberId)
            return Result.WithResult(new FailError("Bad request!"), ResultMode.Exception);

        if (await Repository.ExistsAsync(x => x.MemberId == memberId, cancellationToken))
            return Result.WithResult(new FailError("Bad request!"), ResultMode.Exception);

        var joinRequest = new JoinRequest
        {
            MemberId = memberId,
            Date = DateTime.UtcNow,
            GroupId = parameter.GroupId
        };

        await Repository.AddAsync(joinRequest, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.WithSuccess(new { Message = "Successful" });
    }

    public async Task<Result> ShowJoinRequestToGroupsAsync(int memberId, CancellationToken cancellationToken = new())
    {
        var groups = await _groupRepository.Queryable(false)
            .Where(x => x.Administrator == memberId)
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);

        var joinRequests = await Repository.Queryable(false)
            .Where(x => groups.Contains(x.GroupId))
            .ToListAsync(cancellationToken);

        var values = joinRequests.Select(x => new
        {
            x.Id,
            x.GroupId,
            UserId = x.MemberId,
            Date = x.Date.ToEpochTimeSpan().TotalSeconds
        });

        return values.Any()
            ? Result.WithSuccess(new { JoinRequests = values })
            : Result.WithResult(new FailError("Bad request!"), ResultMode.Exception);
    }

    public async Task<Result> AcceptJoinRequestAsync(int memberId, AcceptJoinRequestDto parameter,
        CancellationToken cancellationToken = new())
    {
        if (await _groupMemberRepository.ExistsAsync(x =>
                x.MemberId == memberId && x.MemberType != MemberType.Administrator, cancellationToken))
        {
            await Repository.DeleteAsync(parameter.JoinRequestId, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.WithResult(new FailError("Bad request!"), ResultMode.Exception);
        }

        var joinRequest = await Repository.FirstOrDefaultAsync(x => x.Id == parameter.JoinRequestId, cancellationToken);

        if (joinRequest == null)
            return Result.WithResult(new FailError("Bad request!"), ResultMode.Exception);

        var groupMember = new GroupMember()
        {
            MemberId = joinRequest.MemberId,
            MemberType = MemberType.Member,
            Id = joinRequest.GroupId
        };

        await _groupMemberRepository.AddAsync(groupMember, cancellationToken);
        await Repository.DeleteAsync(parameter.JoinRequestId, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.WithSuccess(new { Message = "Successful" });
    }
}