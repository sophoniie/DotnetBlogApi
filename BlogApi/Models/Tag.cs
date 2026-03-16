namespace BlogApi.Models;

public class Tag : BaseEntity
{
    public string Name { get; set; } = null!;
    
    public ICollection<ArticleTag> ArticleTags { get; set; } = null!;
}
