using ChatApplication.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ChatApplication.Application.ViewModels.Chat;
using ChatApplication.Application.ViewModels.Authentication;
using Swashbuckle.AspNetCore.Annotations;
using ChatApplication.Domain.Entities;
using System.Collections.Generic;
using ChatApplication.Application.ViewModels.General;

namespace ChatApplication.Controllers;

[Route("api/v1/chats")]
[Authorize]
public class ChatController : HomeController
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    /// <summary>
    /// Get chats
    /// </summary>
    /// <returns></returns>
    /// <response code="200"><h2>important</h2> JWT must contains userId and email in payload</response>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ChatGetDto))]
    [HttpGet]
    public async Task<IActionResult> ShowChatRooms()
    {
        var memberId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value.ToInt32();

        var result = await _chatService.ShowChatRoomsAsync(memberId!.Value);

        return !result.IsSuccess ? BadRequest(result.Value) : Ok(result.Value);
    }

    /// <summary>
    /// Get messages in a chat
    /// </summary>
    /// <param name="memberId"></param>
    /// <returns></returns>
    /// <response code="200">Chat messages (last messages first)</response>
    /// <response code="400">Bad request</response>
    /// <returns></returns>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ChatDetailDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(FailError))]
    [HttpGet("{memberId:int}")]
    public async Task<IActionResult> ShowChatMessages(int memberId)
    {
        var me = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value.ToInt32();

        var result = await _chatService.ShowChatsAsync(me!.Value, memberId);

        return !result.IsSuccess ? BadRequest(result.Value) : Ok(result.Value);
    }

    /// <summary>
    /// Send a message to the user
    /// </summary>
    /// <param name="memberId"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    /// <response code="200">Message sent!</response>
    /// <response code="400">Message isn't delivered! (User is currently not allowed to send message to this user)</response>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(MessageDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(FailError))]
    [HttpPost("{memberId:int}")]
    public async Task<IActionResult> SendMessage(int memberId, SendMessageDto parameter)
    {
        var me = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value.ToInt32();

        var result = await _chatService.SendMessageAsync(me!.Value, memberId, parameter);

        return !result.IsSuccess ? BadRequest(result.Value) : Ok(result.Value);
    }
}