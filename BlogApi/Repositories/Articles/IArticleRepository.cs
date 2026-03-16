using BlogApi.Models;
using BlogApi.Models.Enums;

namespace BlogApi.Repositories.Articles;

public interface IArticleRepository
{
    // Basic queries (no includes)
    Task<Article?> GetByIdAsync(int id);
    Task<Article?> GetBySlugAsync(string slug);
    
    // Detail queries (with all includes)
    Task<Article?> GetByIdWithDetailsAsync(int id);
    Task<Article?> GetBySlugWithDetailsAsync(string slug);
    
    // Paginated lists (no includes)
    Task<IEnumerable<Article>> GetAllAsync(int pageNumber, int pageSize);
    Task<IEnumerable<Article>> GetPublishedAsync(int pageNumber, int pageSize);
    Task<IEnumerable<Article>> GetPublishedBetweenAsync(DateTime from, DateTime to, int pageNumber, int pageSize);
    Task<IEnumerable<Article>> GetByAuthorAsync(int authorId, int pageNumber, int pageSize);
    Task<IEnumerable<Article>> GetByCategoryAsync(int categoryId, int pageNumber, int pageSize);
    Task<IEnumerable<Article>> GetByStatusAsync(ArticleStatusEnum status, int pageNumber, int pageSize);
    
    // Paginated lists with details
    Task<IEnumerable<Article>> GetAllWithDetailsAsync(int pageNumber, int pageSize);
    Task<IEnumerable<Article>> GetPublishedWithDetailsAsync(int pageNumber, int pageSize);
    Task<IEnumerable<Article>> GetPublishedBetweenWithDetailsAsync(DateTime from, DateTime to, int pageNumber, int pageSize);
    
    // Count methods
    Task<int> GetTotalCountAsync();
    Task<int> GetPublishedCountAsync();
    Task<int> GetCountByStatusAsync(ArticleStatusEnum status);
    
    // Deleted articles
    Task<IEnumerable<Article>> GetDeletedAsync(int pageNumber, int pageSize);
    Task<int> GetDeletedCountAsync();
    
    // CRUD
    Task<Article> CreateAsync(Article article);
    Task<Article> UpdateAsync(Article article);
    Task RemoveAsync(int id);
    Task RestoreAsync(int id);
}
