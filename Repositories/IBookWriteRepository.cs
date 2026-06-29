using LibraryApp.Models;
namespace LibraryApp.Repositories;
/// <summary>
/// Defines write data access operations for Book entities.
/// </summary>
public interface IBookWriteRepository
{
    Task AddAsync(Book book);
    Task UpdateAsync(Book book);
    Task DeleteAsync(Book book);
}