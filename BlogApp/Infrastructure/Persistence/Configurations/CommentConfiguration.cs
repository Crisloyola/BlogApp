using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BlogApp.Domain.Entities;

namespace BlogApp.Infrastructure.Persistence.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments");
            builder.HasKey(a => a.Id);

            builder.Property(a => a.CommentDate)
                .IsRequired();

            builder.Property(a => a.Content)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(a => a.Status)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(a => a.UserName)
                .HasMaxLength(100);

            builder.Property(a => a.CreatedAt)
                .IsRequired();

            builder.HasIndex(a => a.PostId);
            builder.HasIndex(a => a.CommentDate);
            builder.HasIndex(a => a.Status);

        }
    }
}