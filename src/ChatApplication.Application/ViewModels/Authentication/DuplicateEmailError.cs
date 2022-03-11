namespace ChatApplication.Application.ViewModels.Authentication;

public class DuplicateEmailError
{
    public DuplicateEmailError(Error error)
    {
        Error = error;
    }

    public Error Error { get; set; }
}