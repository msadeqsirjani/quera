namespace ChatApplication.Application.ViewModels.Authentication;

public class AuthenticationSuccess
{
    public AuthenticationSuccess(string token)
    {
        Token = token;
    }

    public string Token { get; set; }
    public string Message => "Successful";
}