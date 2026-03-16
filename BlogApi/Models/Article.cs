using BlogApi.Models.Enums;

namespace BlogApi.Models;

public class Article : BaseEntity
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public ArticleStatusEnum Status { get; set; }
    public DateTime? PublishedAt { get; set; }
    public string? Thumbnail { get; set; }
    
    public int AuthorId { get; set; }
    public User Author { get; set; } = null!;
    
    public Article()
    {
    }
    
    public Article(string title, string content, User author)
    {
        Title = title;
        Content = content;
        Author = author;
        AuthorId = author.Id;
        Status = ArticleStatusEnum.Draft;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void UpdateInfo(string title, string content)
    {
        Title = title;
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
}
