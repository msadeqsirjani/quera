namespace ChatApplication.Application.ViewModels.Authentication;

public class Error
{
    public Error(string enMessage)
    {
        EnMessage = enMessage;
    }

    public string EnMessage { get; set; }
}