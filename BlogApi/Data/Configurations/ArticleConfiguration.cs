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
        entity.Property(e => e.Content).IsRequired();
        entity.Property(e => e.Status).HasDefaultValue(ArticleStatusEnum.Draft);
    }
}
