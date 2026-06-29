using LibraryApp.Models;

namespace LibraryApp.Repositories;

/// <summary>
/// Defines data access operations for Book entities.
/// </summary>
public interface IBookRepository : IBookReadRepository, IBookWriteRepository
{
    Task<IEnumerable<Book>> GetAllAsync();
    Task<Book?> GetByIdAsync(int bookId);
    Task<IEnumerable<Book>> SearchAsync(string searchTerm);
    Task AddAsync(Book book);
    Task UpdateAsync(Book book);
    Task DeleteAsync(Book book);
}