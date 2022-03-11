using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApplication.Controllers;

[Route("chats")]
[Authorize]
public class ChatController : HomeController
{
    [HttpGet]
    public async Task<IActionResult> ShowChats()
    {
        return Ok();
    }

    [HttpGet("/{uemberId:int}")]
    public async Task<IActionResult> ShowChatMessages(int userId)
    {
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> SendMessageToChat()
    {
        return Ok();
    }
}