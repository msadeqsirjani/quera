using ChatApplication.Application.Services;
using ChatApplication.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ChatApplication.Application.ViewModels.Chat;
using System.Text.RegularExpressions;
using ChatApplication.Application.ViewModels.Authentication;
using Swashbuckle.AspNetCore.Annotations;

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
    [HttpGet("{memberId:int}")]
    public async Task<IActionResult> ShowChatMessages(int memberId)
    {
        var me = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value.ToInt32();

        var result = await _chatService.ShowChatsAsync(me!.Value, memberId);

        return !result.IsSuccess ? BadRequest(result.Value) : Ok(result.Value);
    }

    [HttpPost("{memberId:int}")]
    public async Task<IActionResult> SendMessage(int memberId, SendMessageDto parameter)
    {
        var me = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value.ToInt32();

        var result = await _chatService.SendMessageAsync(me!.Value, memberId, parameter);

        return !result.IsSuccess ? BadRequest(result.Value) : Ok(result.Value);
    }
}