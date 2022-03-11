namespace ChatApplication.Application.ViewModels.Authentication;

public class FailError
{
    public FailError(Error error)
    {
        Error = error;
    }

    public Error Error { get; set; }
}