namespace ChatApplication.Application.ViewModels.ConnectionRequest;

public class ConnectionRequestDto
{
    public int ConnectionRequestId { get; set; }
    public int GroupId { get; set; }
    public int Sent { get; set; }
}

public class ConnectionRequestGetDto
{
    public IEnumerable<ConnectionRequestDto> Requests { get; set; }
}