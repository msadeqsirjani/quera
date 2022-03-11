using ChatApplication.Domain.Common;

namespace ChatApplication.Domain.Entities;

public class Member : Entity
{
    public Member()
    {
        GroupMembers = new List<GroupMember>();
        JoinRequests = new List<JoinRequest>();
        SourceChats = new List<Chat>();
        TargetChats = new List<Chat>();
    }

    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;

    public Group Group { get; set; }
    public ICollection<GroupMember> GroupMembers { get; set; }
    public ICollection<JoinRequest> JoinRequests { get; set; }
    public ICollection<Chat> SourceChats { get; set; }
    public ICollection<Chat> TargetChats { get; set; }
}