namespace BlogApi.DTOs.Articles;

public class UpdateArticleRequest
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string? Thumbnail { get; set; }
}
