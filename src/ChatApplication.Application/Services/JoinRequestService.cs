using ChatApplication.Application.Services.Common;
using ChatApplication.Domain.Entities;
using ChatApplication.Domain.Repositories.Common;

namespace ChatApplication.Application.Services;

public interface IJoinRequestService : IServiceAsync<JoinRequest>
{

}

public class JoinRequestService : ServiceAsync<JoinRequest>, IJoinRequestService
{
    public JoinRequestService(IRepositoryAsync<JoinRequest> repository) : base(repository)
    {
    }
}