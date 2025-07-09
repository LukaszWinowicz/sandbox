using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KERP.Domain.Results;

/// <summary>
/// Represents a specific error with a code and a description.
/// </summary>
/// <param name="Code">An identifier for the error (e.g., "Validation.Required").</param>
/// <param name="Description">A user-friendly description of the error.</param>
public record Error(string Code, string Description)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "The result value is null.");
}
