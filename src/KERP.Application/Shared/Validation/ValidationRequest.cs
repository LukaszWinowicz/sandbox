namespace KERP.Application.Shared.Validation;

public class ValidationRequest<T> where T : class
{
    /// <summary>
    /// Obiekt DTO do walidacji, teraz w pełni typowany.
    /// </summary>
    public T DtoToValidate { get; }

    public List<string> Errors { get; } = new();

    public bool IsValid => !Errors.Any();

    public ValidationRequest(T dtoToValidate)
    {
        DtoToValidate = dtoToValidate;
    }
}
