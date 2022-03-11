using ChatApplication.Domain.Entities;
using ChatApplication.Domain.Repositories;
using ChatApplication.Infra.Data.Common;

namespace ChatApplication.Infra.Data.Repositories;

public class JoinRequestRepository : RepositoryAsync<JoinRequest>, IJoinRequestRepository
{
    public JoinRequestRepository(ApplicationDbContext context) : base(context)
    {
    }
}