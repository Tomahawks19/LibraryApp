using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LibraryApp.Models;

namespace LibraryApp.Data.Configurations;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.HasMany(a => a.Books)
            .WithOne(b => b.Author)
            .HasForeignKey(b => b.AuthorId)
            .IsRequired()
            // Cascade: if an Author is deleted, their Books are deleted too.
            // Justified because a Book without an Author is not valid in this domain
            // (every book must belong to exactly one author).
            .OnDelete(DeleteBehavior.Cascade);
    }
}