using ChatApplication.Application.ViewModels.Group;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ChatApplication.Application.Services;
using ChatApplication.Application.ViewModels.Authentication;
using Swashbuckle.AspNetCore.Annotations;

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
    /// <response code="200">Groups list</response>
    /// <returns></returns>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(GroupGetAllDto))]
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
    /// <response code="200">Group successfully created!</response>
    /// <response code="400">creation failed! (User is already member of a group)</response>
    /// <returns></returns>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(CreateGroupResponseDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(FailError))]
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
    /// <response code="200">Group</response>
    /// <returns></returns>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(MyGroupMemberDto))]
    [HttpGet("my")]
    public async Task<IActionResult> My()
    {
        var memberId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value.ToInt32();

        var result = await _groupService.ShowMyGroupsAsync(memberId!.Value);

        return Ok(result.Value);
    }
}