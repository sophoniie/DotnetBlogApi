using BlogApi.Data;
using BlogApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Repositories.Tags;

public class TagRepository(BlogDbContext context) : ITagRepository
{
    // Basic queries
    public async Task<Tag?> GetByIdAsync(int id)
    {
        return await context.Tags.FindAsync(id);
    }

    public async Task<Tag?> GetByNameAsync(string name)
    {
        return await context.Tags.FirstOrDefaultAsync(t => t.Name == name);
    }
    
    // Paginated lists
    public async Task<IEnumerable<Tag>> GetAllAsync(int pageNumber, int pageSize)
    {
        return await context.Tags
            .OrderBy(t => t.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
    
    // Count
    public async Task<int> GetTotalCountAsync()
    {
        return await context.Tags.CountAsync();
    }
    
    // Deleted
    public async Task<IEnumerable<Tag>> GetDeletedAsync(int pageNumber, int pageSize)
    {
        return await context.Tags
            .IgnoreQueryFilters()
            .Where(t => t.DeletedAt != null)
            .OrderByDescending(t => t.DeletedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetDeletedCountAsync()
    {
        return await context.Tags
            .IgnoreQueryFilters()
            .CountAsync(t => t.DeletedAt != null);
    }
    
    // CRUD
    public async Task<Tag> CreateAsync(Tag tag)
    {
        context.Tags.Add(tag);
        await context.SaveChangesAsync();
        return tag;
    }

    public async Task<Tag> UpdateAsync(Tag tag)
    {
        context.Tags.Update(tag);
        await context.SaveChangesAsync();
        return tag;
    }

    public async Task RemoveAsync(int id)
    {
        var tag = await context.Tags.FindAsync(id);
        if (tag != null)
        {
            tag.Remove();
            await context.SaveChangesAsync();
        }
    }

    public async Task RestoreAsync(int id)
    {
        var tag = await context.Tags
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(t => t.Id == id);

        if (tag != null)
        {
            tag.Restore();
            await context.SaveChangesAsync();
        }
    }
    
    // Unique checks
    public async Task<bool> NameExistsAsync(string name)
    {
        return await context.Tags.AnyAsync(t => t.Name == name);
    }

    public async Task<bool> NameExistsAsync(string name, int? excludeTagId)
    {
        if (excludeTagId.HasValue)
        {
            return await context.Tags.AnyAsync(t => t.Name == name && t.Id != excludeTagId.Value);
        }
        return await context.Tags.AnyAsync(t => t.Name == name);
    }
}
