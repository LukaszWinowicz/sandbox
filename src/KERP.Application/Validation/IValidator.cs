namespace KERP.Application.Validation;

public interface IValidator<T>
{
    ValidationResult Validate(T model);
}