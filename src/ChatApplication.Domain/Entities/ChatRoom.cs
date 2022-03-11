using ChatApplication.Domain.Common;

namespace ChatApplication.Domain.Entities;

public class ChatRoom : Entity
{
    public ChatRoom()
    {
        Chats = new List<Chat>();
    }

    public int SourceMemberId { get; set; }
    public int TargetMemberId { get; set; }

    public Member SourceMember { get; set; }
    public Member TargetMember { get; set; }
    public ICollection<Chat> Chats { get; set; }
}