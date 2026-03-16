using BlogApi.DTOs.Auth;
using BlogApi.DTOs.Users;
using BlogApi.Services.Token;
using BlogApi.Services.Users;

namespace BlogApi.Services.Auth;

public class AuthService(IUserService userService, IPasswordService passwordService, ITokenService tokenService) : IAuthService
{
    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await userService.FindByEmailWithPasswordAsync(request.Email);
        
        if (user == null || !passwordService.Verify(request.Password, user.Password))
        {
            throw new InvalidOperationException("Invalid email or password");
        }

        var userResponse = new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
        
        var token = tokenService.GenerateToken(user);
        
        return new LoginResponse
        {
            Token = token,
            User = userResponse
        };
    }
    
    public async Task<LoginResponse> RegisterAsync(RegisterUserRequest request)
    {
        var user = await userService.CreateAsync(request);
        
        var userModel = await userService.FindByEmailWithPasswordAsync(request.Email);
        
        var token = tokenService.GenerateToken(userModel!);
        
        return new LoginResponse
        {
            Token = token,
            User = user
        };
    }
}
