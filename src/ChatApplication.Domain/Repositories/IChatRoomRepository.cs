using ChatApplication.Domain.Entities;
using ChatApplication.Domain.Repositories.Common;

namespace ChatApplication.Domain.Repositories;

public interface IChatRoomRepository : IRepositoryAsync<ChatRoom>
{
}