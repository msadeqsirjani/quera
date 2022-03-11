using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ChatApplication.Application.Services;
using ChatApplication.Application.ViewModels.ConnectionRequest;
using ChatApplication.Application.ViewModels.Authentication;
using Swashbuckle.AspNetCore.Annotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ChatApplication.Application.ViewModels.General;
using Azure.Core;

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

    /// <summary>
    /// Get connection request of group
    /// </summary>
    /// <response code="200">List of connection requests to the group (Newest first)</response>
    /// <response code="400">Fetch failed (User doesn't have essential permissions)</response>
    /// <returns></returns>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ConnectionRequestGetDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(FailError))]
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
    /// <response code="200">Connection</response>
    /// <response code="400">Request doesn't delivered! (User doesn't have right permissions)</response>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(MessageDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(FailError))]
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
    /// <response code="200">Connection has been accepted</response>
    /// <response code="400">Request doesn't delivered! (User doesn't have right permissions)</response>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(MessageDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(FailError))]
    [HttpPost("accept")]
    public async Task<IActionResult> Accept(AcceptConnectionRequestDto parameter)
    {
        var memberId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value.ToInt32();

        var result = await _connectionRequestService.AcceptConnectionRequestAsync(memberId!.Value, parameter);

        return !result.IsSuccess ? BadRequest(result.Value) : Ok(result.Value);
    }
}