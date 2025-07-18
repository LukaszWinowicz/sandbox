namespace KERP.Application.Abstractions.Validation.Chain;

/// <summary>
/// Abstrakcyjna klasa bazowa dla wszystkich handlerów walidacji.
/// Implementuje wzorzec Chain of Responsibility - każdy handler może:
/// - Wykonać swoją walidację
/// - Przekazać do następnego handlera (jeśli walidacja się powiodła)
/// - Przerwać łańcuch (jeśli walidacja się nie powiodła - fail-fast)
/// </summary>
/// <typeparam name="T">Typ walidowanej wartości</typeparam>
public abstract class BaseChainValidator<T> : IChainValidator<T>
{
    private IChainValidator<T>? _nextHandler;

    /// <inheritdoc />
    public IChainValidator<T> SetNext(IChainValidator<T> handler)
    {
        _nextHandler = handler;
        return handler;
    }

    /// <inheritdoc />
    public async Task<ValidationResult> HandleAsync(
        T value,
        string fieldName,
        CancellationToken cancellationToken = default)
    {
        // Krok 1: Wykonaj własną walidację
        var result = await ValidateAsync(value, fieldName, cancellationToken);

        // Krok 2: Jeśli walidacja się nie powiodła - przerwij łańcuch (fail-fast)
        if (!result.IsValid)
        {
            return result;
        }

        // Krok 3: Jeśli walidacja OK i istnieje następny handler - przekaż dalej
        if (_nextHandler != null)
        {
            return await _nextHandler.HandleAsync(value, fieldName, cancellationToken);
        }

        // Krok 4: Jeśli walidacja OK i brak następnego handlera - zwróć sukces
        return ValidationResult.Success();
    }

    /// <summary>
    /// Metoda do implementacji w konkretnych handlerach.
    /// Zawiera logikę walidacji specyficzną dla danego handlera.
    /// </summary>
    /// <param name="value">Wartość do walidacji</param>
    /// <param name="fieldName">Nazwa pola (do komunikatów błędów)</param>
    /// <param name="cancellationToken">Token anulowania</param>
    /// <returns>Wynik walidacji</returns>
    protected abstract Task<ValidationResult> ValidateAsync(
        T value,
        string fieldName,
        CancellationToken cancellationToken);

}
