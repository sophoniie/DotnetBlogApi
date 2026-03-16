using BlogApi.Models.Enums;
using BlogApi.DTOs.Shared;

namespace BlogApi.DTOs.Articles;

public class ArticleResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string? Excerpt { get; set; }
    public ArticleStatusEnum Status { get; set; }
    public DateTime? PublishedAt { get; set; }
    public string? Thumbnail { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public AuthorSummaryResponse Author { get; set; } = null!;
    public CategorySummaryResponse Category { get; set; } = null!;
    public List<TagSummaryResponse> Tags { get; set; } = new();
}
