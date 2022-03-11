using ChatApplication.Application.ViewModels.Group;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ChatApplication.Application.Services;

namespace ChatApplication.Controllers;

[Route("/api/v1/groups")]
[Authorize]
public class GroupController : HomeController
{
    private readonly IGroupService _groupService;

    public GroupController(IGroupService groupService)
    {
        _groupService = groupService;
    }

    /// <summary>
    /// Get groups
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> ShowGroups()
    {
        var result = await _groupService.ShowAllGroupsAsync();

        return Ok(result.Value);
    }

    /// <summary>
    /// Create a group
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateGroup(CreateGroupDto parameter)
    {
        var memberId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value.ToInt32();

        var result = await _groupService.CreateGroupAsync(memberId!.Value, parameter);

        return !result.IsSuccess ? (IActionResult)BadRequest(result.Value) : (IActionResult)Ok(result.Value);
    }

    /// <summary>
    /// Get detailed information of group of user
    /// </summary>
    /// <returns></returns>
    [HttpGet("my")]
    public async Task<IActionResult> My()
    {
        var memberId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value.ToInt32();

        var result = await _groupService.ShowMyGroupsAsync(memberId!.Value);

        return Ok(result.Value);
    }
}