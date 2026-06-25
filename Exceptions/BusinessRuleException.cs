namespace LibraryApp.Exceptions;

/// <summary>
/// Thrown when an operation violates a library business rule
/// (e.g., checking out a book that is already on loan).
/// </summary>
public class BusinessRuleException : Exception
{
    public string? ResourceName { get; }

    public BusinessRuleException(string message) : base(message)
    {
    }

    public BusinessRuleException(string message, string resourceName) : base(message)
    {
        ResourceName = resourceName;
    }
}