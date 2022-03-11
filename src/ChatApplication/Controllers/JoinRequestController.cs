using ChatApplication.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ChatApplication.Application.ViewModels.Authentication;
using ChatApplication.Application.ViewModels.General;
using ChatApplication.Application.ViewModels.JoinRequest;
using Swashbuckle.AspNetCore.Annotations;

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
    /// <response code="200">List of user's join requests. (Newest first)</response>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(JoinRequestDto))]
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
    /// <response code="200">Request has been sent!</response>
    /// <response code="400">Request failed! (User is already member of a group)</response>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(MessageDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(FailError))]
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
    /// <response code="200">List of user's join requests. (Newest first)</response>
    /// <response code="400">Fetch failed (User doesn't have essential permissions)</response>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(JoinRequestDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(FailError))]
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
    /// <response code="200">User joined the group</response>
    /// <response code="400">Acceptance failed! (User is already member of a group or acceptor doesn't have right permissions to accept request)</response>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(MessageDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(FailError))]
    [HttpPost("accept")]
    public async Task<IActionResult> Accept(AcceptJoinRequestDto parameter)
    {
        var memberId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value.ToInt32();

        var result = await _joinRequestService.AcceptJoinRequestAsync(memberId!.Value, parameter);

        return !result.IsSuccess ? BadRequest(result.Value) : Ok(result.Value);
    }
}