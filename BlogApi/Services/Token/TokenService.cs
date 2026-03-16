using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BlogApi.Models;
using Microsoft.IdentityModel.Tokens;

namespace BlogApi.Services.Token;

public class TokenService : ITokenService
{
    private readonly string _secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") 
        ?? throw new InvalidOperationException("JWT_SECRET_KEY is not configured");
    private readonly string _issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "blog-api";
    private readonly string _audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "blog-api";
    private readonly int _expirationMinutes = int.TryParse(
        Environment.GetEnvironmentVariable("JWT_EXPIRATION_MINUTES"), out var minutes) 
        ? minutes : 30;

    public string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_expirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
