using BlogApi.Models.Enums;

namespace BlogApi.Models;

public class Article : BaseEntity
{
    public string Title { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string? Excerpt { get; set; }
    public ArticleStatusEnum Status { get; set; }
    public DateTime? PublishedAt { get; set; }
    public string? Thumbnail { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    
    public int AuthorId { get; set; }
    public User Author { get; set; } = null!;
    
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    
    public ICollection<ArticleTag> ArticleTags { get; set; } = null!;
    
    public Article()
    {
    }
    
    public Article(string title, string slug, string content, User author)
    {
        Title = title;
        Slug = slug;
        Content = content;
        Author = author;
        AuthorId = author.Id;
        Status = ArticleStatusEnum.Draft;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void UpdateInfo(string title, string slug, string content)
    {
        Title = title;
        Slug = slug;
        Content = content;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void Publish()
    {
        Status = ArticleStatusEnum.Published;
        PublishedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void Unpublish()
    {
        Status = ArticleStatusEnum.Unpublished;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void SetToDraft()
    {
        Status = ArticleStatusEnum.Draft;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void SetThumbnail(string? thumbnail)
    {
        Thumbnail = thumbnail;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void SetExcerpt(string? excerpt)
    {
        Excerpt = excerpt;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void SetMeta(string? title, string? description)
    {
        MetaTitle = title;
        MetaDescription = description;
        UpdatedAt = DateTime.UtcNow;
    }
}
