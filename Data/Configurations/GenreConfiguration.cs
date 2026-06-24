using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LibraryApp.Models;

namespace LibraryApp.Data.Configurations;

public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.Property(g => g.Name)
            .IsRequired()
            .HasMaxLength(100);

        // Seed data: reference genres for both Development and Staging.
        builder.HasData(
            new Genre { GenreId = 2001, Name = "Magical Realism" },
            new Genre { GenreId = 2002, Name = "Historical Fiction" },
            new Genre { GenreId = 2003, Name = "Biography" }
        );
    }
}