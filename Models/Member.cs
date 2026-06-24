using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models;

/// <summary>
/// Represents a library member who can borrow books.
/// </summary>
public class Member
{
    [Key]
    public int MemberId { get; set; }

    [Required]
    [MaxLength(150)]
    public string FullName { get; set; }

    /// <summary>
    /// Email address used for member communications and login.
    /// </summary>
    [MaxLength(200)]
    public string Email { get; set; }

    public bool IsActive { get; set; }

    public char MemberType { get; set; }

    public decimal LateFee { get; set; }

    /// <summary>
    /// Date of birth of the member, used for age verification and demographics.
    /// </summary>
    public DateTime? DateOfBirth { get; set; }
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();

    public Member() { }

    public Member(int memberId, string fullName, bool isActive, char memberType, decimal lateFee)
    {
        MemberId = memberId;
        FullName = fullName;
        IsActive = isActive;
        MemberType = memberType;
        LateFee = lateFee;
    }

    public string GetMemberSummary()
    {
        return $"Member ID: {MemberId} | Name: {FullName} | Active: {IsActive} | Type: {MemberType} | Late Fee: ${LateFee:F2}";
    }
}