using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models;

/// <summary>
/// Represents an author who has written one or more books.
/// </summary>
public class Author
{
    /// <summary>
    /// Primary key for the Author entity.
    /// </summary>
    [Key]
    public int AuthorId { get; set; }

    /// <summary>
    /// Full name of the author.
    /// </summary>
    [Required]
    [MaxLength(150)]
    public string FullName { get; set; }

    /// <summary>
    /// Country of origin of the author.
    /// </summary>
    [MaxLength(100)]
    public string? Nationality { get; set; }

    /// <summary>
    /// Date of birth of the author.
    /// </summary>
    public DateTime? DateOfBirth { get; set; }

    /// <summary>
    /// Short biography of the author.
    /// </summary>
    [MaxLength(1000)]
    public string? Biography { get; set; }

    /// <summary>
    /// Navigation property: all books written by this author.
    /// </summary>
    public ICollection<Book> Books { get; set; } = new List<Book>();
}