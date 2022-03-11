namespace ChatApplication.Application.ViewModels.Chat;

public class ChatGetDto
{
    public int UserId { get; set; }
    public string Name { get; set; }
}

public class ChatDto
{
    public string Message { get; set; }
    public double Date { get; set; }
    public int SendBy { get; set; }
}

public class ChatDetailDto
{
    public IEnumerable<ChatDto> Messages { get; set; }
}