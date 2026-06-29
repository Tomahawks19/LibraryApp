using LibraryApp.Models;
namespace LibraryApp.Repositories;
/// <summary>
/// Defines data access operations for Member entities.
/// </summary>
public interface IMemberRepository
{
    Task<IEnumerable<Member>> GetAllAsync();
    Task<Member?> GetByIdAsync(int memberId);
    Task AddAsync(Member member);
    Task UpdateAsync(Member member);
}