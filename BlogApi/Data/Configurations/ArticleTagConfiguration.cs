using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BlogApi.Models;

namespace BlogApi.Data.Configurations;

public class ArticleTagConfiguration : IEntityTypeConfiguration<ArticleTag>
{
    public void Configure(EntityTypeBuilder<ArticleTag> entity)
    {
        entity.HasKey(e => new { e.ArticleId, e.TagId });
        
        entity.HasOne(e => e.Article)
              .WithMany(e => e.ArticleTags)
              .HasForeignKey(e => e.ArticleId)
              .OnDelete(DeleteBehavior.Cascade);
        
        entity.HasOne(e => e.Tag)
              .WithMany(e => e.ArticleTags)
              .HasForeignKey(e => e.TagId)
              .OnDelete(DeleteBehavior.Cascade);
    }
}
