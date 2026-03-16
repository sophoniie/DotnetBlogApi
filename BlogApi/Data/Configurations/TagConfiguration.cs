using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BlogApi.Models;

namespace BlogApi.Data.Configurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> entity)
    {
        entity.HasKey(e => e.Id);
        
        entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
        
        entity.HasIndex(e => e.Name).IsUnique();
        
        entity.HasQueryFilter(e => e.DeletedAt == null);
    }
}
