namespace KERP.Application.Abstractions.Validation;

/// <summary>
/// Reprezentuje pojedynczą, atomową regułę walidacyjną dla właściwości określonego typu.
/// </summary>
/// <typeparam name="TProperty">Typ właściwości podlegającej walidacji.</typeparam>
public interface IValidationRule<in TProperty>
{
    /// <summary>
    /// Wykonuje walidację dla podanej wartości.
    /// </summary>
    /// <param name="value">Wartość do zwalidowania.</param>
    /// <param name="cancellationToken">Token do anulowania operacji.</param>
    /// <returns>
    /// Komunikat błędu, jeśli walidacja nie powiodła się; w przeciwnym razie null.
    /// </returns>
    Task<string?> ValidateAsync(TProperty value, CancellationToken cancellationToken = default);
}