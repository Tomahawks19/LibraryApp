using LibraryApp.Models;

namespace LibraryApp.Repositories
{
    /// <summary>
    /// Defines read-only data access operations for Book entities.
    /// </summary>
    public interface IBookReadRepository
    {
        Task<IEnumerable<Book>> GetAllAsync();
        Task<Book?> GetByIdAsync (int bookId);
        Task<IEnumerable<Book>> SearchAsync(string serachTerm);
    }
}
