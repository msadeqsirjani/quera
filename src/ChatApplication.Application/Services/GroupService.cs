using ChatApplication.Application.Services.Common;
using ChatApplication.Domain.Entities;
using ChatApplication.Domain.Repositories.Common;

namespace ChatApplication.Application.Services;

public interface IGroupService : IServiceAsync<Group>
{

}

public class GroupService : ServiceAsync<Group>, IGroupService
{
    public GroupService(IRepositoryAsync<Group> repository) : base(repository)
    {
    }
}