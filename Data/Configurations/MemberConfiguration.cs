using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LibraryApp.Models;

namespace LibraryApp.Data.Configurations;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.HasMany(m => m.Loans)
            .WithOne(l => l.Member)
            .HasForeignKey(l => l.MemberId)
            .IsRequired()
            // Restrict: prevents deleting a Member who still has Loan records,
            // protecting the historical borrowing record from being silently destroyed.
            .OnDelete(DeleteBehavior.Restrict);
    }
}