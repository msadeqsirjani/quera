using ChatApplication.Application.Services.Common;
using ChatApplication.Domain.Entities;
using ChatApplication.Domain.Repositories.Common;

namespace ChatApplication.Application.Services;

public interface IConnectionRequestService : IServiceAsync<ConnectionRequest>
{

}
public class ConnectionRequestService : ServiceAsync<ConnectionRequest>, IConnectionRequestService
{
    public ConnectionRequestService(IRepositoryAsync<ConnectionRequest> repository) : base(repository)
    {
    }
}