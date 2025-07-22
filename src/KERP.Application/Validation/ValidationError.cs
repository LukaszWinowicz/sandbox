namespace KERP.Application.Validation;

public record ValidationError(string PropertyName, string ErrorMessage);
