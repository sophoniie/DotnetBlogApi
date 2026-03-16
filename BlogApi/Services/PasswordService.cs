using BCryptNet = BCrypt.Net.BCrypt;

namespace BlogApi.Services;

public class PasswordService : IPasswordService
{
    public string Hash(string password)
    {
        return BCryptNet.HashPassword(password, BCryptNet.GenerateSalt(12));
    }
    
    public bool Verify(string password, string hash)
    {
        return BCryptNet.Verify(password, hash);
    }
}
