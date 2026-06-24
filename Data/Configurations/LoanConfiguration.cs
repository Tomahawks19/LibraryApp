using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LibraryApp.Models;

namespace LibraryApp.Data.Configurations;

public class LoanConfiguration : IEntityTypeConfiguration<Loan>
{
    public void Configure(EntityTypeBuilder<Loan> builder)
    {
        // Relationships for Loan are configured from the Member and Book side
        // (see MemberConfiguration and BookConfiguration) to avoid duplicate
        // configuration of the same relationship from both ends.
        builder.Property(l => l.LoanDate)
            .IsRequired();
    }
}