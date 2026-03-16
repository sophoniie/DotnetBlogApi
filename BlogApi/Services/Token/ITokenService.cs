using BlogApi.Models;

namespace BlogApi.Services.Token;

public interface ITokenService
{
    string GenerateToken(User user);
}