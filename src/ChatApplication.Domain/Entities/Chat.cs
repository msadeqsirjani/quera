using ChatApplication.Domain.Common;

namespace ChatApplication.Domain.Entities;

public class Chat : Entity
{
    public int ChatRoomId { get; set; }
    public string Message { get; set; } = null!;
    public DateTime Date { get; set; }

    public ChatRoom ChatRoom { get; set; }
}