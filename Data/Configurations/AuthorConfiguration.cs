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
            .OnDelete(DeleteBehavior.Cascade);

        // Seed data: reference authors available in Development.
        // Fixed AuthorId values are required by HasData() since EF Core
        // cannot auto-generate keys for seed data at migration-build time.
        builder.HasData(
            new Author { AuthorId = 1001, FullName = "Gabriel García Márquez", Nationality = "Colombian" },
            new Author { AuthorId = 1002, FullName = "Isabel Allende", Nationality = "Chilean" },
            new Author { AuthorId = 1003, FullName = "Mario Vargas Llosa", Nationality = "Peruvian" }
        );
    }
}