using BlogApi.DTOs.Auth;
using BlogApi.DTOs.Users;

namespace BlogApi.Services.Auth;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<LoginResponse> RegisterAsync(RegisterUserRequest request);
}