using ChatApplication.Application.Services.Common;
using ChatApplication.Domain.Entities;
using ChatApplication.Domain.Repositories.Common;

namespace ChatApplication.Application.Services;

public interface IGroupMemberService : IServiceAsync<GroupMember>
{

}

public class GroupMemberService : ServiceAsync<GroupMember>, IGroupMemberService
{
    public GroupMemberService(IRepositoryAsync<GroupMember> repository) : base(repository)
    {
    }
}