using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApplication.Controllers;

[Route("/api/v1/groups")]
[Authorize]
public class GroupController : HomeController
{
    [HttpGet]
    public async Task<IActionResult> ShowGroups()
    {
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> CreateGroup()
    {
        return Ok();
    }

    [HttpPost("my")]
    public async Task<IActionResult> My()
    {
        return Ok();
    }
}