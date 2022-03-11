using ChatApplication.Application.Services.Common;
using ChatApplication.Domain.Entities;
using ChatApplication.Domain.Repositories.Common;

namespace ChatApplication.Application.Services;

public interface IChatService : IServiceAsync<Chat>
{

}

public class ChatService : ServiceAsync<Chat>, IChatService
{
    public ChatService(IRepositoryAsync<Chat> repository) : base(repository)
    {
    }
}