namespace LibraryApp.Exceptions;

/// <summary>
/// Thrown when a requested entity (book, member, etc.) does not exist.
/// </summary>
public class NotFoundException : Exception
{
    /// <summary>
    /// Identifies which entity type triggered this exception (e.g. "Book", "Member").
    /// </summary>
    public string? ResourceName { get; }

    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string message, string resourceName) : base(message)
    {
        ResourceName = resourceName;
    }
}