namespace LibraryApp.Exceptions;

/// <summary>
/// Thrown when an operation violates a library business rule
/// (e.g., checking out a book that is already on loan).
/// </summary>
public class BusinessRuleException : Exception
{
    public BusinessRuleException(string message) : base(message) { }
}