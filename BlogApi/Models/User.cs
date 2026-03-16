using BlogApi.Models.Enums;

namespace BlogApi.Models;

public class User : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public RoleEnum Role { get; set; } = RoleEnum.Reader;
    public bool IsActive { get; set; } = true;
    public DateTime? DeletedAt { get; set; }
    
    public ICollection<Article> Articles { get; set; } = null!;
    
    public User()
    {
    }
    
    public User(string name, string email, string password, RoleEnum role = RoleEnum.Reader)
    {
        Name = name;
        Email = email;
        Password = password;
        Role = role;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void UpdateInfo(string name, string email)
    {
        Name = name;
        Email = email;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void ChangeRole(RoleEnum role)
    {
        Role = role;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void UpdatePassword(string password)
    {
        // TODO: Hash password before storing (e.g., using BCrypt, ASP.NET Core Identity, or similar)
        Password = password;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void Disable()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void Enable()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void SoftDelete()
    {
        DeletedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void Restore()
    {
        DeletedAt = null;
        UpdatedAt = DateTime.UtcNow;
    }
}
