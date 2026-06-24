namespace LibraryApp.Exceptions;

/// <summary>
/// Thrown when a requested entity (book, member, etc.) does not exist.
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}