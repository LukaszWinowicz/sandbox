namespace KERP.Application.Abstractions.Validation.Chain;

/// <summary>
/// Interfejs dla handlera w łańcuchu walidacji (Chain of Responsibility pattern).
/// Każdy handler może walidować wartość i przekazać ją do następnego handlera.
/// </summary>
/// <typeparam name="T">Typ walidowanej wartości</typeparam>
public interface IChainValidator<T>
{
    /// <summary>
    /// Ustawia następny handler w łańcuchu.
    /// </summary>
    /// <param name="handler">Następny handler do wykonania</param>
    /// <returns>Następny handler (dla łatwego łączenia w łańcuch)</returns>
    IChainValidator<T> SetNext(IChainValidator<T> nextHandler);

    /// <summary>
    /// Wykonuje walidację wartości. Jeśli walidacja przejdzie pomyślnie,
    /// przekazuje wartość do następnego handlera (jeśli istnieje).
    /// </summary>
    /// <param name="value">Wartość do walidacji</param>
    /// <param name="fieldName">Nazwa pola (do komunikatów błędów)</param>
    /// <param name="cancellationToken">Token anulowania operacji</param>
    /// <returns>Wynik walidacji</returns>
    Task<ValidationResult> HandleAsync(
        T value,
        string fieldName,
        CancellationToken cancellationToken = default);
}
