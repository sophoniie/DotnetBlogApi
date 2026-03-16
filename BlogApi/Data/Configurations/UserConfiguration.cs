using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BlogApi.Models;
using BlogApi.Models.Enums;

namespace BlogApi.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity.HasKey(e => e.Id);
        
        entity.Property(e => e.Name).IsRequired();
        entity.Property(e => e.Email).IsRequired();
        entity.Property(e => e.Password).IsRequired();
        entity.Property(e => e.Role).HasDefaultValue(RoleEnum.Reader);
        entity.Property(e => e.IsActive).HasDefaultValue(true);
        
        entity.HasIndex(e => e.Email).IsUnique();
        
        entity.HasMany(e => e.Articles)
              .WithOne(e => e.Author)
              .HasForeignKey(e => e.AuthorId)
              .OnDelete(DeleteBehavior.Restrict);
    }
}
