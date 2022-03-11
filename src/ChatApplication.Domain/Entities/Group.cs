using ChatApplication.Domain.Common;

namespace ChatApplication.Domain.Entities;

public class Group : Entity
{
    public Group()
    {
        GroupMembers = new List<GroupMember>();
        SourceConnectionRequests  = new List<ConnectionRequest>();
        JoinRequests = new List<JoinRequest>();
    }

    public int Administrator { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public Member Member { get; set; }
    public ICollection<GroupMember> GroupMembers { get; set; }
    public ICollection<ConnectionRequest> SourceConnectionRequests { get; set; }
    public ICollection<JoinRequest> JoinRequests { get; set; }
}