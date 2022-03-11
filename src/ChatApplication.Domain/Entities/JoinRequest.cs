using ChatApplication.Domain.Common;

namespace ChatApplication.Domain.Entities;

public class JoinRequest : Entity
{
    public int GroupId { get; set; }
    public int MemberId { get; set; }
    public DateTime Date { get; set; }

    public Group Group { get; set; } = null!;
    public Member Member { get; set; } = null!;
}