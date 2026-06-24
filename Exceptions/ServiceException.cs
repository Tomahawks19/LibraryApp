namespace LibraryApp.Exceptions;

/// <summary>
/// Wraps unexpected lower-level failures (e.g., repository/database errors)
/// so the service layer exposes a consistent exception type to its callers.
/// </summary>
public class ServiceException : Exception
{
    public ServiceException(string message, Exception innerException)
        : base(message, innerException) { }
}