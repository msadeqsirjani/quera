using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApplication.Controllers;

[Route("connection-requests")]
[Authorize]
public class ConnectionRequestController : HomeController
{
    [HttpGet]
    public async Task<IActionResult> ShowConnectionRequest()
    {
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> SendConnectionRequest()
    {
        return Ok();
    }

    [HttpPost("accept")]
    public async Task<IActionResult> Accept()
    {
        return Ok();
    }
}