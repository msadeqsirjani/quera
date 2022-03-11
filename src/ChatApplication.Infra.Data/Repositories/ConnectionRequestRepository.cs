using ChatApplication.Domain.Entities;
using ChatApplication.Domain.Repositories;
using ChatApplication.Infra.Data.Common;

namespace ChatApplication.Infra.Data.Repositories;

public class ConnectionRequestRepository : RepositoryAsync<ConnectionRequest>, IConnectionRequestRepository
{
    public ConnectionRequestRepository(ApplicationDbContext context) : base(context)
    {
    }
}