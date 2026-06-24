using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models;

/// <summary>
/// Represents a genre/category that can be applied to multiple books.
/// </summary>
public class Genre
{
    /// <summary>
    /// Primary key for the Genre entity.
    /// </summary>
    [Key]
    public int GenreId { get; set; }

    /// <summary>
    /// Name of the genre (e.g., "Fiction", "Science", "History").
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    /// <summary>
    /// Navigation property: all books associated with this genre.
    /// </summary>
    public ICollection<Book> Books { get; set; } = new List<Book>();
}