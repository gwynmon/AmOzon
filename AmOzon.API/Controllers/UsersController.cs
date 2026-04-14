using AmOzon.API.Extensions;
using AmOzon.Application.Abstractions;
using AmOzon.Application.Commands;
using AmOzon.Contracts.Requests;
using AmOzon.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using Mapster;
using Microsoft.AspNetCore.Authorization;

namespace AmOzon.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService, IProfileService profileService) : ControllerBase
{
    [HttpPost("create")]
    public async Task<ActionResult<Guid>> CreateUser([FromBody] UserRequest request)
    {
        var command = request.Adapt<CreateUserCommand>();
        var userId = await userService.CreateUserAsync(command);
        return Ok(userId); 
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<UserResponse>> GetThisUsersProfile()
    {
        var userId = User.GetUserId();
        var profile = await profileService.GetUserProfile(userId);
        return Ok(profile);
    }

    [HttpGet("get-all")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<UserResponse>>> GetAllUsers()
    {
        var users = await userService.GetAllUsers();
        var response = users.Adapt<List<UserResponse>>();
        return Ok(response);
    }

    [HttpGet("get/{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserResponse>> GetUserById(Guid id)
    {
        var user = await userService.GetUser(id);
        var response = user.Adapt<UserResponse>();
        return Ok(response);
    }
}