using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryApp.Models;

/// <summary>
/// Represents a record of a member borrowing a specific book.
/// </summary>
public class Loan
{
    /// <summary>
    /// Primary key for the Loan entity.
    /// </summary>
    [Key]
    public int LoanId { get; set; }

    /// <summary>
    /// Date the book was checked out.
    /// </summary>
    public DateTime LoanDate { get; set; }

    /// <summary>
    /// Date the book is due back, or was returned.
    /// </summary>
    public DateTime? ReturnDate { get; set; }

    /// <summary>
    /// Foreign key referencing the Member who took out this loan.
    /// </summary>
    [ForeignKey(nameof(Member))]
    public int MemberId { get; set; }

    /// <summary>
    /// Navigation property: the member who took out this loan.
    /// </summary>
    public Member Member { get; set; }

    /// <summary>
    /// Foreign key referencing the Book that was borrowed.
    /// </summary>
    [ForeignKey(nameof(Book))]
    public int BookId { get; set; }

    /// <summary>
    /// Navigation property: the book that was borrowed.
    /// </summary>
    public Book Book { get; set; }
}