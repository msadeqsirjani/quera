using ChatApplication.Application.Constants;
using ChatApplication.Application.Services.Common;
using ChatApplication.Application.Utilities;
using ChatApplication.Application.ViewModels.Authentication;
using ChatApplication.Domain.Common;
using ChatApplication.Domain.Entities;
using ChatApplication.Domain.Repositories.Common;

namespace ChatApplication.Application.Services;

public interface IMemberService : IServiceAsync<Member>
{
    Task<Result> SignInAsync(SignInDto parameter, CancellationToken cancellationToken = new());
    Task<Result> LoginAsync(LoginDto parameter, CancellationToken cancellationToken = new());
}

public class MemberService : ServiceAsync<Member>, IMemberService
{
    private readonly IJwtService _jwtService;
    private readonly IUnitOfWorkAsync _unitOfWork;

    public MemberService(IRepositoryAsync<Member> repository, IJwtService jwtService, IUnitOfWorkAsync unitOfWork) :
        base(repository)
    {
        _jwtService = jwtService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> SignInAsync(SignInDto parameter,
        CancellationToken cancellationToken = new())
    {
        if (await Repository.ExistsAsync(x => x.Email == parameter.Email, cancellationToken))
            return Result.WithException(new FailError(Statement.Failure));

        var member = new Member
        {
            Name = parameter.Name!,
            Email = parameter.Email!,
            Password = Security.Encrypt(parameter.Password!)
        };

        await Repository.AddAsync(member, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var token = _jwtService.GenerateJwtToken(member.Id, member.Email!);

        return Result.WithSuccess(new AuthenticationSuccess(token));
    }

    public async Task<Result> LoginAsync(LoginDto parameter, CancellationToken cancellationToken = new())
    {
        var member = await Repository.FirstOrDefaultAsync(x => x.Email == parameter.Email, cancellationToken);

        if(member == null || member.Password != Security.Encrypt(parameter.Password!))
            return Result.WithException(new FailError(Statement.Failure));

        var token = _jwtService.GenerateJwtToken(member.Id, member.Email);

        return Result.WithSuccess(new AuthenticationSuccess(token));
    }
}