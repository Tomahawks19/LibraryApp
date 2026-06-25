using LibraryApp.Data;
using LibraryApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Repositories;

/// <summary>
/// EF Core implementation of IAuthorRepository.
/// </summary>
public class AuthorRepository : IAuthorRepository
{
    private readonly LibraryContext _context;

    public AuthorRepository(LibraryContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Author>> GetAllAsync()
    {
        return await _context.Authors
            .AsNoTracking()
            .OrderBy(a => a.FullName)
            .ToListAsync();
    }

    public async Task<Author?> GetByIdAsync(int authorId)
    {
        return await _context.Authors.FindAsync(authorId);
    }
}