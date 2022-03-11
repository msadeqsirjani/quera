using ChatApplication.Domain.Entities;
using ChatApplication.Domain.Repositories;
using ChatApplication.Infra.Data.Common;

namespace ChatApplication.Infra.Data.Repositories;

public class MemberRepository : RepositoryAsync<Member>, IMemberRepository
{
    public MemberRepository(ApplicationDbContext context) : base(context)
    {
    }
}