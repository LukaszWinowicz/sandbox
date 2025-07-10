namespace KERP.Application.Shared.Validation;

/// <summary>
/// Reprezentuje pojedyncze ogniwo w łańcuchu walidacyjnym (Chain of Responsibility).
/// </summary>
public interface IValidationHandler<T> where T : class
{
    IValidationHandler<T> SetNext(IValidationHandler<T> nextHandler);

    // Metoda przyjmuje generyczny obiekt żądania
    Task ValidateAsync(ValidationRequest<T> request);
}