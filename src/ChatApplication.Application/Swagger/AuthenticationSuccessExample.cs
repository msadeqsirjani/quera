using ChatApplication.Application.ViewModels.Authentication;
using Swashbuckle.AspNetCore.Filters;

namespace ChatApplication.Application.Swagger;

public class AuthenticationSuccessExample : IExamplesProvider<AuthenticationSuccess>
{
    public AuthenticationSuccess GetExamples()
    {
        return new AuthenticationSuccess("hello");
    }
}