namespace KERP.Application.Common.Validation.Rules;

/// <summary>
/// Interfejs bazowy dla wszystkich reguł walidacji.
/// Reprezentuje pojedynczą regułę która może być zastosowana do wartości typu T.
/// </summary>
/// <typeparam name="T">Typ walidowanej wartości (np. string, int, DateTime)</typeparam>
public interface IValidationRule<in T>
{
    /// <summary>
    /// Komunikat błędu wyświetlany gdy walidacja nie przejdzie.
    /// Powinien być zrozumiały dla użytkownika końcowego.
    /// </summary>
    string ErrorMessage { get; }

    /// <summary>
    /// Sprawdza czy wartość spełnia regułę walidacji.
    /// </summary>
    /// <param name="value">Wartość do walidacji</param>
    /// <returns>True jeśli wartość jest poprawna, false jeśli reguła została złamana</returns>
    bool Validate(T value);
}
