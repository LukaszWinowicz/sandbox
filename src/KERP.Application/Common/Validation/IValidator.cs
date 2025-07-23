namespace KERP.Application.Common.Validation;

public interface IValidator<T>
{
    ValidationResult Validate(T model);
}