using BlogApi.Data;
using BlogApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Repositories.Users;

public class UserRepository(BlogDbContext context) : IUserRepository
{
    public async Task<User?> GetByIdAsync(int id)
    {
        return await context.Users.FindAsync(id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<User>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
    {
        return await context.Users
            .OrderBy(u => u.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await context.Users.CountAsync();
    }

    public async Task<IEnumerable<User>> GetDeletedAsync(int pageNumber = 1, int pageSize = 10)
    {
        return await context.Users
            .IgnoreQueryFilters()
            .Where(u => u.DeletedAt != null)
            .OrderByDescending(u => u.DeletedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetDeletedCountAsync()
    {
        return await context.Users
            .IgnoreQueryFilters()
            .CountAsync(u => u.DeletedAt != null);
    }

    public async Task<User> CreateAsync(User user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateAsync(User user)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task RemoveAsync(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user != null)
        {
            user.SoftDelete();
            await context.SaveChangesAsync();
        }
    }

    public async Task RestoreAsync(int id)
    {
        var user = await context.Users
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Id == id);
        
        if (user != null)
        {
            user.Restore();
            await context.SaveChangesAsync();
        }
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<bool> EmailExistsAsync(string email, int? excludeUserId)
    {
        if (excludeUserId.HasValue)
        {
            return await context.Users.AnyAsync(u => u.Email == email && u.Id != excludeUserId.Value);
        }
        return await context.Users.AnyAsync(u => u.Email == email);
    }
}
