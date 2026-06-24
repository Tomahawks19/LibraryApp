namespace LibraryApp.Dtos;

/// <summary>
/// Lightweight projection of book data used for paginated search results.
/// </summary>
public class BookSummaryDto
{
    public string Title { get; set; }
    public int PublicationYear { get; set; }
    public string AuthorName { get; set; }
}