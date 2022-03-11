namespace ChatApplication.Application.ViewModels.Group;

public class CreateGroupResponseDto
{
    public GroupDto Group { get; set; }

    public class GroupDto
    {
        public int Id { get; set; }
    }
}