using System.Collections;
using ChatApplication.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ChatApplication.Application.Extensions;
using ChatApplication.Application.ViewModels.JoinRequest;

namespace ChatApplication.Controllers;

[Route("api/v1/join-requests")] 
[Authorize]
public class JoinRequestController : HomeController
{
    private readonly IJoinRequestService _joinRequestService;

    public JoinRequestController(IJoinRequestService joinRequestService)
    {
        _joinRequestService = joinRequestService;
    }

    /// <summary>
    /// Get join requests of user
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> ShowJoinRequest()
    {
        var memberId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value.ToInt32();

        var result = await _joinRequestService.ShowMyJoinRequestAsync(memberId!.Value);

        return !result.IsSuccess ? BadRequest(result.Value) : Ok(result.Value);
    }

    /// <summary>
    /// Send join request to a group
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> SendJoinRequest(SendJoinRequestDto parameter)
    {
        var memberId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value.ToInt32();

        var result = await _joinRequestService.SendJoinRequestAsync(memberId!.Value, parameter);

        return !result.IsSuccess ? (IActionResult)BadRequest(result.Value) : (IActionResult)Ok(result.Value);
    }

    /// <summary>
    /// Get join requests of user's group
    /// </summary>
    /// <returns></returns>
    [HttpGet("group")]
    public async Task<IActionResult> Group()
    {
        var memberId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value.ToInt32();

        var result = await _joinRequestService.ShowJoinRequestToGroupsAsync(memberId!.Value);

        return !result.IsSuccess ? BadRequest(result.Value) : Ok(result.Value);
    }

    /// <summary>
    /// Accept a join request to user's group
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    [HttpPost("accept")]
    public async Task<IActionResult> Accept(AcceptJoinRequestDto parameter)
    {
        var memberId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value.ToInt32();

        var result = await _joinRequestService.AcceptJoinRequestAsync(memberId!.Value, parameter);

        return !result.IsSuccess ? BadRequest(result.Value) : Ok(result.Value);
    }
}