using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BlogApi.Models;
using BlogApi.Models.Enums;

namespace BlogApi.Data.Configurations;

public class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> entity)
    {
        entity.HasKey(e => e.Id);
        
        entity.Property(e => e.Title).IsRequired();
        entity.Property(e => e.Slug).IsRequired().HasMaxLength(200);
        entity.Property(e => e.Content).IsRequired();
        entity.Property(e => e.Excerpt).HasMaxLength(300);
        entity.Property(e => e.Status).HasDefaultValue(ArticleStatusEnum.Draft);
        entity.Property(e => e.MetaTitle).HasMaxLength(70);
        entity.Property(e => e.MetaDescription).HasMaxLength(160);
        
        entity.HasIndex(e => e.Slug).IsUnique();
        
        entity.HasOne(e => e.Author)
              .WithMany(e => e.Articles)
              .HasForeignKey(e => e.AuthorId)
              .OnDelete(DeleteBehavior.Restrict);
        
        entity.HasOne(e => e.Category)
              .WithMany(e => e.Articles)
              .HasForeignKey(e => e.CategoryId)
              .OnDelete(DeleteBehavior.Restrict);
        
        entity.HasMany(e => e.ArticleTags)
              .WithOne(e => e.Article)
              .HasForeignKey(e => e.ArticleId)
              .OnDelete(DeleteBehavior.Cascade);
        
        entity.HasQueryFilter(e => e.DeletedAt == null);
    }
}
