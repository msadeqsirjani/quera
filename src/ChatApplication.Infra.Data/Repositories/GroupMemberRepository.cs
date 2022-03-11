using ChatApplication.Domain.Entities;
using ChatApplication.Domain.Repositories;
using ChatApplication.Infra.Data.Common;

namespace ChatApplication.Infra.Data.Repositories;

public class GroupMemberRepository : RepositoryAsync<GroupMember>, IGroupMemberRepository
{
    public GroupMemberRepository(ApplicationDbContext context) : base(context)
    {
    }
}