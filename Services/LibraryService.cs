using LibraryApp.Exceptions;
using LibraryApp.Models;
using LibraryApp.Repositories;

namespace LibraryApp.Services;

/// <summary>
/// Encapsulates library business logic, coordinating between
/// the book and loan repositories. Contains no direct EF Core or SQL knowledge.
/// </summary>
public class LibraryService
{
    private readonly IBookRepository _bookRepository;
    private readonly ILoanRepository _loanRepository;

    private const int OverdueDaysThreshold = 14;

    public LibraryService(IBookRepository bookRepository, ILoanRepository loanRepository)
    {
        _bookRepository = bookRepository;
        _loanRepository = loanRepository;
    }

    /// <summary>
    /// Returns the full book catalog.
    /// </summary>
    public async Task<IEnumerable<Book>> GetBookCatalogAsync()
    {
        try
        {
            return await _bookRepository.GetAllAsync();
        }
        catch (Exception ex) when (ex is not NotFoundException && ex is not BusinessRuleException)
        {
            throw new ServiceException("Failed to retrieve the book catalog.", ex);
        }
    }

    /// <summary>
    /// Searches the catalog by a partial title match.
    /// </summary>
    public async Task<IEnumerable<Book>> SearchBooksAsync(string searchTerm)
    {
        if (searchTerm is null)
            throw new ArgumentNullException(nameof(searchTerm));

        if (string.IsNullOrWhiteSpace(searchTerm))
            throw new ArgumentException("Search term cannot be empty or whitespace.", nameof(searchTerm));

        try
        {
            return await _bookRepository.SearchAsync(searchTerm);
        }
        catch (Exception ex) when (ex is not NotFoundException && ex is not BusinessRuleException)
        {
            throw new ServiceException("Failed to search the book catalog.", ex);
        }
    }

    /// <summary>
    /// Returns the loan history for a given member.
    /// </summary>
    public async Task<IEnumerable<Loan>> GetMemberLoanHistoryAsync(int memberId)
    {
        return await _loanRepository.GetByMemberIdAsync(memberId);
    }

    /// <summary>
    /// Returns all loans currently overdue.
    /// </summary>
    public async Task<IEnumerable<Loan>> GetOverdueLoansReportAsync()
    {
        return await _loanRepository.GetOverdueLoansAsync(OverdueDaysThreshold);
    }

    /// <summary>
    /// Checks out a book to a member, enforcing business rules.
    /// </summary>
    public async Task<Loan> CheckOutBookAsync(int bookId, int memberId)
    {
        if (memberId <= 0)
            throw new ArgumentException("Member id must be a positive number.", nameof(memberId));

        var book = await _bookRepository.GetByIdAsync(bookId);
        if (book is null)
            throw new NotFoundException($"Book with id {bookId} was not found.");

        var member = await _loanRepository.GetMemberByIdAsync(memberId);
        if (member is null)
            throw new NotFoundException($"Member with id {memberId} was not found.");

        if (!book.IsAvailable)
            throw new BusinessRuleException($"Book '{book.Title}' is already on loan.");

        var loan = new Loan
        {
            BookId = bookId,
            MemberId = memberId,
            LoanDate = DateTime.Now
        };

        book.IsAvailable = false;
        await _bookRepository.UpdateAsync(book);
        await _loanRepository.AddAsync(loan);

        return loan;
    }
}