namespace MassUpdate.Core.DTOs;

public class RowValidationResult
{
    public int RowNumber { get; }
    public List<string> Errors { get; }
    public bool IsValid => !Errors.Any();

    public RowValidationResult(int rowNumber, List<string> errors)
    {
        RowNumber = rowNumber;
        Errors = errors;
    }
}


