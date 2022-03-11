namespace ChatApplication.Application.ViewModels.Authentication;

public class FailError
{
    public FailError(string message)
    {
        Error = new Error(message);
    }

    public Error Error { get; set; }
}