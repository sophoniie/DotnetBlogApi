using BlogApi.DTOs.Users;

namespace BlogApi.DTOs.Auth;

public class LoginResponse
{
    public string Token { get; set; } = null!;
    public UserResponse User { get; set; } = null!;
}
