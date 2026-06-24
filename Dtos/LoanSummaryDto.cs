namespace LibraryApp.Dtos;

/// <summary>
/// Lightweight projection of loan data used for reporting and member portal views.
/// </summary>
public class LoanSummaryDto
{
    public string MemberName { get; set; }
    public string BookTitle { get; set; }
    public DateTime LoanDate { get; set; }
    public bool IsOverdue { get; set; }
}