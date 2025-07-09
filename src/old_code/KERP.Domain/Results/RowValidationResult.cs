namespace KERP.Domain.Results;

/// <summary>
/// Represents the validation outcome for a single row in a mass update process.
/// </summary>
public class RowValidationResult
{
    /// <summary>
    /// The original row number from the user's input.
    /// </summary>
    public int RowNumber { get; }

    /// <summary>
    /// A list of validation error messages for this specific row.
    /// </summary>
    public List<string> Errors { get; }

    /// <summary>
    /// A convenience property to quickly check if the row is valid.
    /// </summary>
    public bool IsValid => !Errors.Any();

    public RowValidationResult(int rowNumber, List<string> errors)
    {
        RowNumber = rowNumber;
        Errors = errors;
    }
}