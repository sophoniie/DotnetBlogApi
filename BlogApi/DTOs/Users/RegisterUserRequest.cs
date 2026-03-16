using BlogApi.Models.Enums;

namespace BlogApi.DTOs.Users;

public class RegisterUserRequest
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public RoleEnum? Role { get; set; }
}
