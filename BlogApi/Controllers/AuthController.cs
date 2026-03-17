using BlogApi.DTOs.Auth;
using BlogApi.DTOs.Users;
using BlogApi.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<LoginResponse>> Register(RegisterUserRequest request)
    {
        var result = await authService.RegisterAsync(request);
        return Ok(result);
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        var result = await authService.LoginAsync(request);
        return Ok(result);
    }
}