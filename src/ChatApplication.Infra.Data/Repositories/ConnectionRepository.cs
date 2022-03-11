using ChatApplication.Domain.Entities;
using ChatApplication.Domain.Repositories;
using ChatApplication.Infra.Data.Common;

namespace ChatApplication.Infra.Data.Repositories;

public class ConnectionRepository : RepositoryAsync<ConnectionRequest>, IConnectionRequestRepository
{
    public ConnectionRepository(ApplicationDbContext context) : base(context)
    {
    }
}