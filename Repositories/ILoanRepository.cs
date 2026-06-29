using LibraryApp.Models;
namespace LibraryApp.Repositories;
/// <summary>
/// Defines data access operations for Loan entities.
/// </summary>
public interface ILoanRepository
{
    Task<IEnumerable<Loan>> GetByMemberIdAsync(int memberId);
    Task<Loan?> GetByIdAsync(int loanId);
    Task<IEnumerable<Loan>> GetOverdueLoansAsync(int overdueDaysThreshold);
    Task<Member?> GetMemberByIdAsync(int memberId);
    Task AddAsync(Loan loan);
    Task UpdateAsync(Loan loan);
}