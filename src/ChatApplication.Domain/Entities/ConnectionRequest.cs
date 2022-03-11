using ChatApplication.Domain.Common;

namespace ChatApplication.Domain.Entities;

public class ConnectionRequest : Entity
{
    public int SourceGroupId { get; set; }
    public int TargetGroupId { get; set; }
    public DateTime Date { get; set; }

    public Group SourceGroup { get; set; }
}