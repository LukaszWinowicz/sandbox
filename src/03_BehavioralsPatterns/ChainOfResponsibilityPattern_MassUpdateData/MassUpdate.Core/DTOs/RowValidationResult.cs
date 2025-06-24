namespace MassUpdate.Core.DTOs;

public class RowValidationResult
{
    public int RowNumber { get; }
    public List<string> ValidationErrors { get; }
    public bool IsValid => ValidationErrors.Count == 0;

    public RowValidationResult(int rowNumber, List<string> validationErrors)
    {
        RowNumber = rowNumber;
        ValidationErrors = validationErrors;
    }
}


