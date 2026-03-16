namespace BlogApi.DTOs.Articles;

public class CreateArticleRequest
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public int AuthorId { get; set; }
    public string? Thumbnail { get; set; }
}
