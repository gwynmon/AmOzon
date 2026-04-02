using AmOzon.Application.Abstractions;
using AmOzon.Application.Commands;
using AmOzon.Contracts;
using AmOzon.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Mapster;

namespace AmOzon.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpPost("create")]
    public async Task<ActionResult<Guid>> CreateUser([FromBody] UserRequest request)
    {
        var command = request.Adapt<CreateUserCommand>();
        var sellerId = await userService.CreateUserAsync(command);
        return Ok(sellerId); 
    }

    [HttpGet("get-all")]
    public async Task<ActionResult<List<UserResponse>>> GetAllUsers()
    {
        var users = await userService.GetAllUsers();
        var response = users.Value.Adapt<List<UserResponse>>();
        return Ok(response);
    }

    [HttpGet("get/{id:guid}")]
    public async Task<ActionResult<UserResponse>> GetUserById(Guid id)
    {
        var user = await userService.GetUser(id);
        var response = user.Adapt<UserResponse>();
        return Ok(response);
    }
}