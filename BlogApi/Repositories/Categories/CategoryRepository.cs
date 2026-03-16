using BlogApi.Data;
using BlogApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Repositories.Categories;

public class CategoryRepository(BlogDbContext context) : ICategoryRepository
{
    // Basic queries
    public async Task<Category?> GetByIdAsync(int id)
    {
        return await context.Categories.FindAsync(id);
    }

    public async Task<Category?> GetBySlugAsync(string slug)
    {
        return await context.Categories.FirstOrDefaultAsync(c => c.Slug == slug);
    }
    
    // Paginated lists
    public async Task<IEnumerable<Category>> GetAllAsync(int pageNumber, int pageSize)
    {
        return await context.Categories
            .OrderBy(c => c.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Category>> GetAllWithArticlesCountAsync(int pageNumber, int pageSize)
    {
        return await context.Categories
            .Include(c => c.Articles)
            .Select(c => new Category
            {
                Id = c.Id,
                Name = c.Name,
                Slug = c.Slug,
                Description = c.Description,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                Articles = c.Articles
            })
            .OrderBy(c => c.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
    
    // Count
    public async Task<int> GetTotalCountAsync()
    {
        return await context.Categories.CountAsync();
    }
    
    // Deleted
    public async Task<IEnumerable<Category>> GetDeletedAsync(int pageNumber, int pageSize)
    {
        return await context.Categories
            .IgnoreQueryFilters()
            .Where(c => c.DeletedAt != null)
            .OrderByDescending(c => c.DeletedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetDeletedCountAsync()
    {
        return await context.Categories
            .IgnoreQueryFilters()
            .CountAsync(c => c.DeletedAt != null);
    }
    
    // CRUD
    public async Task<Category> CreateAsync(Category category)
    {
        context.Categories.Add(category);
        await context.SaveChangesAsync();
        return category;
    }

    public async Task<Category> UpdateAsync(Category category)
    {
        context.Categories.Update(category);
        await context.SaveChangesAsync();
        return category;
    }

    public async Task RemoveAsync(int id)
    {
        var category = await context.Categories.FindAsync(id);
        if (category != null)
        {
            category.Remove();
            await context.SaveChangesAsync();
        }
    }

    public async Task RestoreAsync(int id)
    {
        var category = await context.Categories
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category != null)
        {
            category.Restore();
            await context.SaveChangesAsync();
        }
    }
    
    // Unique checks
    public async Task<bool> SlugExistsAsync(string slug)
    {
        return await context.Categories.AnyAsync(c => c.Slug == slug);
    }

    public async Task<bool> SlugExistsAsync(string slug, int? excludeCategoryId)
    {
        if (excludeCategoryId.HasValue)
        {
            return await context.Categories.AnyAsync(c => c.Slug == slug && c.Id != excludeCategoryId.Value);
        }
        return await context.Categories.AnyAsync(c => c.Slug == slug);
    }
}
