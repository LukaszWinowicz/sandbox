namespace KERP.Application.Common.Validation.Rules;

/// <summary>
/// Interfejs dla reguł walidacji wymagających operacji asynchronicznych.
/// Używany głównie dla reguł sprawdzających dane w bazie danych lub zewnętrznych serwisach.
/// </summary>
/// <typeparam name="T">Typ walidowanej wartości</typeparam>
public interface IAsyncValidationRule<in T> : IValidationRule<T>
{
    /// <summary>
    /// Asynchronicznie sprawdza czy wartość spełnia regułę walidacji.
    /// </summary>
    /// <param name="value">Wartość do walidacji</param>
    /// <param name="cancellationToken">Token do anulowania operacji</param>
    /// <returns>Task zwracający true jeśli wartość jest poprawna, false jeśli reguła została złamana</returns>
    Task<bool> ValidateAsync(T value, CancellationToken cancellationToken = default);

    /// <summary>
    /// Synchroniczna wersja walidacji. 
    /// Domyślnie rzuca wyjątek - należy używać ValidateAsync.
    /// </summary>
    bool IValidationRule<T>.Validate(T value)
    {
        throw new NotSupportedException(
            $"Reguła {GetType().Name} wymaga użycia ValidateAsync. Synchroniczna walidacja nie jest wspierana.");
    }
}
