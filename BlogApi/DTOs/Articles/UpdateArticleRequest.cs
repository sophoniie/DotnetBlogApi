namespace BlogApi.DTOs.Articles;

public class UpdateArticleRequest
{
    public string Title { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string? Excerpt { get; set; }
    public int CategoryId { get; set; }
    public string? Thumbnail { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public List<int> TagIds { get; set; } = new();
}
