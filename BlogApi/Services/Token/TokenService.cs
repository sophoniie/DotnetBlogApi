using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BlogApi.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BlogApi.Services.Token;

public class TokenService(IConfiguration configuration) : ITokenService
{
    private readonly string _secretKey = configuration["Jwt:Key"] 
        ?? throw new InvalidOperationException("Jwt:Key is not configured");
    private readonly string _issuer = configuration["Jwt:Issuer"] 
        ?? throw new InvalidOperationException("Jwt:Issuer is not configured");
    private readonly string _audience = configuration["Jwt:Audience"] 
        ?? throw new InvalidOperationException("Jwt:Audience is not configured");
    private readonly int _expirationMinutes = int.TryParse(
        configuration["Jwt:ExpirationMinutes"], out var minutes) 
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
