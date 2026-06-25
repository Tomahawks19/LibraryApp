namespace LibraryApp.Exceptions;

/// <summary>
/// Thrown when input data fails validation that cannot be caught
/// by standard model annotation attributes.
/// </summary>
public class ValidationException : Exception
{
    public string? ResourceName { get; }

    public ValidationException(string message) : base(message)
    {
    }

    public ValidationException(string message, string resourceName) : base(message)
    {
        ResourceName = resourceName;
    }
}