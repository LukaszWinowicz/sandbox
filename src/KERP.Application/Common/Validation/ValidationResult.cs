namespace KERP.Application.Common.Validation;

public class ValidationResult
{
    public bool IsValid => !Errors.Any();
    public List<ValidationError> Errors { get; }

    public ValidationResult(List<ValidationError> errors)
    {
        Errors = errors;
    }
}
