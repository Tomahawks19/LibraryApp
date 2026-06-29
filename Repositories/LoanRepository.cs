using LibraryApp.Data;
using LibraryApp.Models;
using Microsoft.EntityFrameworkCore;
namespace LibraryApp.Repositories;
/// <summary>
/// EF Core implementation of ILoanRepository.
/// </summary>
public class LoanRepository : ILoanRepository
{
    private readonly LibraryContext _context;
    public LoanRepository(LibraryContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Loan>> GetByMemberIdAsync(int memberId)
    {
        return await _context.Loans
            .AsNoTracking()
            .Include(l => l.Book)
            .Where(l => l.MemberId == memberId)
            .ToListAsync();
    }
    public async Task<Loan?> GetByIdAsync(int loanId)
    {
        return await _context.Loans
            .Include(l => l.Book)
            .FirstOrDefaultAsync(l => l.LoanId == loanId);
    }
    public async Task<IEnumerable<Loan>> GetOverdueLoansAsync(int overdueDaysThreshold)
    {
        var cutoff = DateTime.Now.AddDays(-overdueDaysThreshold);
        return await _context.Loans
            .AsNoTracking()
            .Include(l => l.Member)
            .Include(l => l.Book)
            .Where(l => l.ReturnDate == null && l.LoanDate < cutoff)
            .ToListAsync();
    }
    public async Task<Member?> GetMemberByIdAsync(int memberId)
    {
        return await _context.Members.FindAsync(memberId);
    }
    public async Task AddAsync(Loan loan)
    {
        _context.Loans.Add(loan);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(Loan loan)
    {
        _context.Loans.Update(loan);
        await _context.SaveChangesAsync();
    }
}