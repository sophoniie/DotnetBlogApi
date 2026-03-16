namespace BlogApi.Models;

public class Tag : BaseEntity
{
    public string Name { get; set; } = null!;
    public DateTime? DeletedAt { get; set; }
    
    public ICollection<ArticleTag> ArticleTags { get; set; } = null!;
    
    public void Remove()
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
