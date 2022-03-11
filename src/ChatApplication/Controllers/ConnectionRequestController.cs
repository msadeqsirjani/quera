using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ChatApplication.Application.Services;
using ChatApplication.Application.ViewModels.ConnectionRequest;
using System.Reflection.Metadata;

namespace ChatApplication.Controllers;

[Route("connection-requests")]
[Authorize]
public class ConnectionRequestController : HomeController
{
    private readonly IConnectionRequestService _connectionRequestService;

    public ConnectionRequestController(IConnectionRequestService connectionRequestService)
    {
        _connectionRequestService = connectionRequestService;
    }

    [HttpGet]
    public async Task<IActionResult> ShowConnectionRequest()
    {
        var memberId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value.ToInt32();

        var result = await _connectionRequestService.ShowConnectionRequestAsync(memberId!.Value);

        return !result.IsSuccess ? BadRequest(result.Value) : Ok(result.Value);
    }

    /// <summary>
    /// Send a connection request
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> SendConnectionRequest(SendConnectionRequestDto parameter)
    {
        var memberId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value.ToInt32();

        var result = await _connectionRequestService.SendConnectionRequestAsync(memberId!.Value, parameter);

        return !result.IsSuccess ? BadRequest(result.Value) : Ok(result.Value);
    }

    /// <summary>
    /// Accept a connection request
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    [HttpPost("accept")]
    public async Task<IActionResult> Accept(AcceptConnectionRequestDto parameter)
    {
        var memberId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value.ToInt32();

        var result = await _connectionRequestService.AcceptConnectionRequestAsync(memberId!.Value, parameter);

        return !result.IsSuccess ? BadRequest(result.Value) : Ok(result.Value);
    }
}