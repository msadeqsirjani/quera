using ChatApplication.Application.Services.Common;
using ChatApplication.Domain.Entities;
using ChatApplication.Domain.Repositories.Common;

namespace ChatApplication.Application.Services;

public interface IChatRoomService : IServiceAsync<ChatRoom>
{

}

public class ChatRoomService : ServiceAsync<ChatRoom>, IChatRoomService
{
    public ChatRoomService(IRepositoryAsync<ChatRoom> repository) : base(repository)
    {
    }
}