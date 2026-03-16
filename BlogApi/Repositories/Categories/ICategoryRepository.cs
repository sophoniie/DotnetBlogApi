using BlogApi.Models;

namespace BlogApi.Repositories.Categories;

public interface ICategoryRepository
{
    // Basic queries
    Task<Category?> GetByIdAsync(int id);
    Task<Category?> GetBySlugAsync(string slug);
    
    // Paginated lists
    Task<IEnumerable<Category>> GetAllAsync(int pageNumber, int pageSize);
    Task<IEnumerable<Category>> GetAllWithArticlesCountAsync(int pageNumber, int pageSize);
    
    // Count
    Task<int> GetTotalCountAsync();
    
    // Deleted
    Task<IEnumerable<Category>> GetDeletedAsync(int pageNumber, int pageSize);
    Task<int> GetDeletedCountAsync();
    
    // CRUD
    Task<Category> CreateAsync(Category category);
    Task<Category> UpdateAsync(Category category);
    Task RemoveAsync(int id);
    Task RestoreAsync(int id);
    
    // Unique checks
    Task<bool> SlugExistsAsync(string slug);
    Task<bool> SlugExistsAsync(string slug, int? excludeCategoryId);
}
