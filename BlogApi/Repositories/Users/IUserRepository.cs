using BlogApi.Models;

namespace BlogApi.Repositories.Users;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetAllAsync(int pageNumber = 1, int pageSize = 10);
    Task<int> GetTotalCountAsync();
    Task<IEnumerable<User>> GetDeletedAsync(int pageNumber = 1, int pageSize = 10);
    Task<int> GetDeletedCountAsync();
    Task<User> CreateAsync(User user);
    Task<User> UpdateAsync(User user);
    Task RemoveAsync(int id);
    Task RestoreAsync(int id);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> EmailExistsAsync(string email, int? excludeUserId);
}
