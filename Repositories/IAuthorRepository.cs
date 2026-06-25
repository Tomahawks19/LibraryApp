using LibraryApp.Models;

namespace LibraryApp.Repositories;

/// <summary>
/// Defines data access operations for Author entities.
/// </summary>
public interface IAuthorRepository
{
    Task<IEnumerable<Author>> GetAllAsync();
    Task<Author?> GetByIdAsync(int authorId);
}