using ChatApplication.Application.Constants;
using ChatApplication.Application.Exceptions;
using ChatApplication.Application.Services;
using ChatApplication.Application.ViewModels.Authentication;
using ChatApplication.Domain.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace ChatApplication.Infra.IoC.MiddleWares;

public class ExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogService _logService;
    private readonly IWebHostEnvironment _environment;

    public ExceptionHandler(RequestDelegate next, ILogService logService, IWebHostEnvironment environment)
    {
        _next = next;
        _logService = logService;
        _environment = environment;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 200;


        switch (exception)
        {
            case UnAuthorizedException unAuthorizedException:
                {
                    context.Response.StatusCode = 400;

                    return context.Response.WriteAsync(
                        JsonConvert.SerializeObject(Result.WithException(unAuthorizedException.Message)));
                }
            default:
                _logService.LogError(exception);

                return context.Response.WriteAsync(_environment.IsDevelopment()
                    ? JsonConvert.SerializeObject(new FailError(new Error("Bad request!")))
                    : JsonConvert.SerializeObject(Result.WithException(Statement.Failure)));
        }
    }
}