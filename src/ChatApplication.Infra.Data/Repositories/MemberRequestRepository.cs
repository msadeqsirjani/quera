using ChatApplication.Domain.Entities;
using ChatApplication.Domain.Repositories;
using ChatApplication.Infra.Data.Common;

namespace ChatApplication.Infra.Data.Repositories;

public class MemberRequestRepository : RepositoryAsync<Member>, IMemberRepository
{
    public MemberRequestRepository(ApplicationDbContext context) : base(context)
    {
    }
}