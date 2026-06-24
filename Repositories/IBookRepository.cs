using LibraryApp.Models;

namespace LibraryApp.Repositories;

/// <summary>
/// Defines data access operations for Book entities.
/// </summary>
public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllAsync();
    Task<Book?> GetByIdAsync(int bookId);
    Task<IEnumerable<Book>> SearchAsync(string searchTerm);
    Task UpdateAsync(Book book);
}