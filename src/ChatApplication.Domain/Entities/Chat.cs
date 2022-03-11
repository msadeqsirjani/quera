using ChatApplication.Domain.Common;

namespace ChatApplication.Domain.Entities;

public class Chat : Entity
{
    public int SourceMemberId { get; set; }
    public int TargetMemberId { get; set; }
    public string Message { get; set; } = null!;

    public Member SourceMember { get; set; } = null!;
    public Member TargetMember { get; set; } = null!;

}