using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LibraryApp.Models;

namespace LibraryApp.Data.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        // Book -> Loan (one-to-many)
        builder.HasMany(b => b.Loans)
            .WithOne(l => l.Book)
            .HasForeignKey(l => l.BookId)
            .IsRequired()
            // Restrict: a Book with existing Loan history should not be deletable
            // outright, to preserve the historical loan record.
            .OnDelete(DeleteBehavior.Restrict);

        // Book <-> Genre (many-to-many) with explicit join table
        builder.HasMany(b => b.Genres)
            .WithMany(g => g.Books)
            .UsingEntity<Dictionary<string, object>>(
                "BookGenre",
                right => right
                    .HasOne<Genre>()
                    .WithMany()
                    .HasForeignKey("GenreId")
                    .OnDelete(DeleteBehavior.Cascade),
                left => left
                    .HasOne<Book>()
                    .WithMany()
                    .HasForeignKey("BookId")
                    .OnDelete(DeleteBehavior.Cascade),
                join =>
                {
                    join.HasKey("BookId", "GenreId");
                    join.Property<DateTime>("DateTagged").HasDefaultValueSql("GETDATE()");
                    join.ToTable("BookGenre");
                });
    }
}