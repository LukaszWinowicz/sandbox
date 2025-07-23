namespace KERP.Application.Common.Validation.Rules.Common;

/// <summary>
/// Reguła sprawdzająca czy string ma dokładnie określoną długość.
/// Używana np. dla kodów które muszą mieć stałą długość (kody pocztowe, numery dokumentów).
/// </summary>
public class ExactLengthRule : IValidationRule<string>
{
    private readonly int _requiredLength;

    /// <summary>
    /// Komunikat błędu wyświetlany gdy długość jest nieprawidłowa.
    /// </summary>
    public string ErrorMessage { get; }

    /// <summary>
    /// Tworzy nową instancję reguły ExactLength.
    /// </summary>
    /// <param name="requiredLength">Wymagana dokładna długość string'a</param>
    /// <param name="errorMessage">Opcjonalny własny komunikat błędu</param>
    /// <exception cref="ArgumentOutOfRangeException">Gdy requiredLength jest mniejsze od 0</exception>
    public ExactLengthRule(int requiredLength, string? errorMessage = null)
    {
        if (requiredLength < 0)
            throw new ArgumentOutOfRangeException(nameof(requiredLength),
                "Długość nie może być ujemna");

        _requiredLength = requiredLength;
        ErrorMessage = errorMessage ?? $"Pole musi mieć dokładnie {requiredLength} znaków";
    }

    /// <summary>
    /// Sprawdza czy wartość ma dokładnie wymaganą długość.
    /// Null jest traktowany jako błąd (długość 0 != null).
    /// </summary>
    /// <param name="value">String do sprawdzenia</param>
    /// <returns>True jeśli długość jest dokładnie taka jak wymagana</returns>
    public bool Validate(string value)
    {
        // Null nie przechodzi walidacji - użyj NotEmptyRule jeśli pole może być puste
        if (value == null)
            return false;

        return value.Length == _requiredLength;
    }
}
