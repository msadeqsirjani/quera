using ChatApplication.Application.ViewModels.GroupMember;

namespace ChatApplication.Application.ViewModels.Group;

public class MyGroupGetDto
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public IEnumerable<GroupMemberDto> Members { get; set; }
}