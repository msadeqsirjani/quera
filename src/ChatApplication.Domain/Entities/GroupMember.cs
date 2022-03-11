using ChatApplication.Domain.Common;
using ChatApplication.Domain.Enums;

namespace ChatApplication.Domain.Entities;

public class GroupMember : Entity
{
    public int MemberId { get; set; }
    public MemberType MemberType { get; set; }

    public Group Group { get; set; } = null!;
    public Member Member { get; set; } = null!;
}