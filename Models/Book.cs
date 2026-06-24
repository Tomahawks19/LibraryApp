using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryApp.Models;

/// <summary>
/// Represents a book available in the library catalog.
/// </summary>
public class Book
{
    /// <summary>
    /// Primary key for the Book entity.
    /// </summary>
    [Key]
    public int BookId { get; set; }

    /// <summary>
    /// Title of the book.
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Title { get; set; }

    /// <summary>
    /// Year the book was originally published.
    /// </summary>
    public int PublicationYear { get; set; }

    /// <summary>
    /// Exact publication date of the book, if known (more precise than PublicationYear).
    /// </summary>
    public DateTime? PublishedDate { get; set; }

    /// <summary>
    /// Number of pages in the book.
    /// </summary>
    public int PageCount { get; set; }

    /// <summary>
    /// Indicates whether the book is currently available to borrow.
    /// </summary>
    public bool IsAvailable { get; set; } = true;

    /// <summary>
    /// Foreign key referencing the Author who wrote this book.
    /// </summary>
    [ForeignKey(nameof(Author))]
    public int AuthorId { get; set; }

    /// <summary>
    /// Navigation property: the author who wrote this book.
    /// </summary>
    public Author Author { get; set; }

    /// <summary>
    /// International Standard Book Number for this book.
    /// </summary>
    [MaxLength(20)]
    public string? ISBN { get; set; }

    /// <summary>
    /// Navigation property: all loan records involving this book.
    /// </summary>
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();

    /// <summary>
    /// Navigation property: all genres associated with this book.
    /// </summary>
    public ICollection<Genre> Genres { get; set; } = new List<Genre>();
}