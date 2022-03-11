using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApplication.Controllers;

[Route("api/v1/join-requests")] 
[Authorize]
public class JoinRequestController : HomeController
{
    [HttpGet]
    public async Task<IActionResult> ShowJoinRequest()
    {
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> SendJoinRequest()
    {
        return Ok();
    }

    [HttpGet("group")]
    public async Task<IActionResult> Group()
    {
        return Ok();
    }

    [HttpPost("accept")]
    public async Task<IActionResult> Accept()
    {
        return Ok();
    }
}