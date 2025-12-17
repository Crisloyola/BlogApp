using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BlogApp.Domain.Entities;



namespace BlogApp.Infrastructure.Persistence.Configurations
{
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.ToTable("Authors");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(o => o.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(o => o.Phone)
                .HasMaxLength(15);

            builder.Property(o => o.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(o => o.Email)
                .IsUnique();

            builder.HasMany(o => o.Posts)
                .WithOne(p => p.Author)
                .HasForeignKey(p => p.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}