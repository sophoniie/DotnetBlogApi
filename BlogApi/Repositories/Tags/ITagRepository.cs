using BlogApi.Models;

namespace BlogApi.Repositories.Tags;

public interface ITagRepository
{
    // Basic queries
    Task<Tag?> GetByIdAsync(int id);
    Task<Tag?> GetByNameAsync(string name);
    
    // Paginated lists
    Task<IEnumerable<Tag>> GetAllAsync(int pageNumber, int pageSize);
    
    // Count
    Task<int> GetTotalCountAsync();
    
    // Deleted
    Task<IEnumerable<Tag>> GetDeletedAsync(int pageNumber, int pageSize);
    Task<int> GetDeletedCountAsync();
    
    // CRUD
    Task<Tag> CreateAsync(Tag tag);
    Task<Tag> UpdateAsync(Tag tag);
    Task RemoveAsync(int id);
    Task RestoreAsync(int id);
    
    // Unique checks
    Task<bool> NameExistsAsync(string name);
    Task<bool> NameExistsAsync(string name, int? excludeTagId);
}
