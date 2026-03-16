using BlogApi.Data;
using BlogApi.Models;
using BlogApi.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Repositories.Articles;

public class ArticleRepository(BlogDbContext context) : IArticleRepository
{
    // Basic queries (no includes)
    public async Task<Article?> GetByIdAsync(int id)
    {
        return await context.Articles.FindAsync(id);
    }

    public async Task<Article?> GetBySlugAsync(string slug)
    {
        return await context.Articles.FirstOrDefaultAsync(a => a.Slug == slug);
    }
    
    // Detail queries (with all includes)
    public async Task<Article?> GetByIdWithDetailsAsync(int id)
    {
        return await context.Articles
            .Include(a => a.Author)
            .Include(a => a.Category)
            .Include(a => a.ArticleTags)
                .ThenInclude(at => at.Tag)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Article?> GetBySlugWithDetailsAsync(string slug)
    {
        return await context.Articles
            .Include(a => a.Author)
            .Include(a => a.Category)
            .Include(a => a.ArticleTags)
                .ThenInclude(at => at.Tag)
            .FirstOrDefaultAsync(a => a.Slug == slug);
    }
    
    // Paginated lists (no includes)
    public async Task<IEnumerable<Article>> GetAllAsync(int pageNumber, int pageSize)
    {
        return await context.Articles
            .OrderByDescending(a => a.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Article>> GetPublishedAsync(int pageNumber, int pageSize)
    {
        return await context.Articles
            .Where(a => a.Status == ArticleStatusEnum.Published)
            .OrderByDescending(a => a.PublishedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Article>> GetPublishedBetweenAsync(DateTime from, DateTime to, int pageNumber, int pageSize)
    {
        return await context.Articles
            .Where(a => a.Status == ArticleStatusEnum.Published 
                     && a.PublishedAt >= from 
                     && a.PublishedAt <= to)
            .OrderByDescending(a => a.PublishedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Article>> GetByAuthorAsync(int authorId, int pageNumber, int pageSize)
    {
        return await context.Articles
            .Where(a => a.AuthorId == authorId)
            .OrderByDescending(a => a.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Article>> GetByCategoryAsync(int categoryId, int pageNumber, int pageSize)
    {
        return await context.Articles
            .Where(a => a.CategoryId == categoryId)
            .OrderByDescending(a => a.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Article>> GetByStatusAsync(ArticleStatusEnum status, int pageNumber, int pageSize)
    {
        return await context.Articles
            .Where(a => a.Status == status)
            .OrderByDescending(a => a.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
    
    // Paginated lists with details
    public async Task<IEnumerable<Article>> GetAllWithDetailsAsync(int pageNumber, int pageSize)
    {
        return await context.Articles
            .Include(a => a.Author)
            .Include(a => a.Category)
            .Include(a => a.ArticleTags)
                .ThenInclude(at => at.Tag)
            .OrderByDescending(a => a.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Article>> GetPublishedWithDetailsAsync(int pageNumber, int pageSize)
    {
        return await context.Articles
            .Include(a => a.Author)
            .Include(a => a.Category)
            .Include(a => a.ArticleTags)
                .ThenInclude(at => at.Tag)
            .Where(a => a.Status == ArticleStatusEnum.Published)
            .OrderByDescending(a => a.PublishedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Article>> GetPublishedBetweenWithDetailsAsync(DateTime from, DateTime to, int pageNumber, int pageSize)
    {
        return await context.Articles
            .Include(a => a.Author)
            .Include(a => a.Category)
            .Include(a => a.ArticleTags)
                .ThenInclude(at => at.Tag)
            .Where(a => a.Status == ArticleStatusEnum.Published 
                     && a.PublishedAt >= from 
                     && a.PublishedAt <= to)
            .OrderByDescending(a => a.PublishedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
    
    // Count methods
    public async Task<int> GetTotalCountAsync()
    {
        return await context.Articles.CountAsync();
    }

    public async Task<int> GetPublishedCountAsync()
    {
        return await context.Articles
            .CountAsync(a => a.Status == ArticleStatusEnum.Published);
    }

    public async Task<int> GetCountByStatusAsync(ArticleStatusEnum status)
    {
        return await context.Articles
            .CountAsync(a => a.Status == status);
    }
    
    // Deleted articles
    public async Task<IEnumerable<Article>> GetDeletedAsync(int pageNumber, int pageSize)
    {
        return await context.Articles
            .IgnoreQueryFilters()
            .Where(a => a.DeletedAt != null)
            .OrderByDescending(a => a.DeletedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetDeletedCountAsync()
    {
        return await context.Articles
            .IgnoreQueryFilters()
            .CountAsync(a => a.DeletedAt != null);
    }
    
    // CRUD
    public async Task<Article> CreateAsync(Article article)
    {
        context.Articles.Add(article);
        await context.SaveChangesAsync();
        return article;
    }

    public async Task<Article> UpdateAsync(Article article)
    {
        context.Articles.Update(article);
        await context.SaveChangesAsync();
        return article;
    }

    public async Task RemoveAsync(int id)
    {
        var article = await context.Articles.FindAsync(id);
        if (article != null)
        {
            article.Remove();
            await context.SaveChangesAsync();
        }
    }

    public async Task RestoreAsync(int id)
    {
        var article = await context.Articles
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(a => a.Id == id);

        if (article != null)
        {
            article.Restore();
            await context.SaveChangesAsync();
        }
    }
}
