using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BlogApi.Models;

namespace BlogApi.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> entity)
    {
        entity.HasKey(e => e.Id);
        
        entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
        entity.Property(e => e.Slug).IsRequired().HasMaxLength(150);
        entity.Property(e => e.Description).HasMaxLength(500);
        
        entity.HasIndex(e => e.Slug).IsUnique();
    }
}
