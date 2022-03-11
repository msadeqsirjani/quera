using ChatApplication.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> ShowChats()
    {
        return Ok();
    }

    [HttpGet("{memberId:int}")]
    public async Task<IActionResult> ShowChatMessages(int memberId)
    {
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> SendMessageToChat()
    {
        return Ok();
    }
}