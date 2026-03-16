namespace BlogApi.Models;

public class Category : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string? Description { get; set; }
    
    public ICollection<Article> Articles { get; set; } = null!;
}
