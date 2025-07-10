namespace KERP.Application.Shared.Validation;

/// <summary>
/// Abstrakcyjna klasa bazowa dla ogniw łańcucha walidacyjnego.
/// Obsługuje logikę przekazywania żądania do następnego ogniwa.
/// </summary>
public abstract class AbstractValidationHandler<T> : IValidationHandler<T> where T : class
{
    private IValidationHandler<T>? _nextHandler;

    public IValidationHandler<T> SetNext(IValidationHandler<T> nextHandler)
    {
        _nextHandler = nextHandler;
        return nextHandler; // Zwracamy nextHandler, aby umożliwić płynne łączenie
    }

    public virtual async Task ValidateAsync(ValidationRequest<T> request)
    {
        // Krok 1: Wykonaj logikę walidacji zaimplementowaną w klasie dziedziczącej.
        await HandleValidation(request);

        // Krok 2: Jeśli istnieje następne ogniwo, przekaż mu żądanie.
        if (_nextHandler != null)
        {
            await _nextHandler.ValidateAsync(request);
        }
    }

    /// <summary>
    /// Metoda, którą muszą zaimplementować konkretne walidatory.
    /// Zawiera ich specyficzną logikę walidacyjną.
    /// </summary>
    protected abstract Task HandleValidation(ValidationRequest<T> request);
}
