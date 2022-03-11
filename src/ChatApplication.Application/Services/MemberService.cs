using ChatApplication.Application.Services.Common;
using ChatApplication.Domain.Entities;
using ChatApplication.Domain.Repositories.Common;

namespace ChatApplication.Application.Services;

public interface IMemberService : IServiceAsync<Member>
{

}

public class MemberService : ServiceAsync<Member>, IMemberService
{
    public MemberService(IRepositoryAsync<Member> repository) : base(repository)
    {
    }
}