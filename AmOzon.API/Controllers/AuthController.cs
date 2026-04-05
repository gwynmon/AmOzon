using AmOzon.Application.Abstractions;
using AmOzon.Contracts.Requests;
using AmOzon.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace AmOzon.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        var result = await authService.LoginAsync(request);
        
        if (result == null)
            return Unauthorized(new { message = "Invalid email or password" });

        return Ok(result);
    }
}