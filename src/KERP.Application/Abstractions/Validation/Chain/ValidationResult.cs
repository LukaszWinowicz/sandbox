namespace KERP.Application.Abstractions.Validation.Chain;

/// <summary>
/// Reprezentuje wynik walidacji - czy jest poprawna i ewentualny komunikat błędu
/// </summary>
public class ValidationResult
{
    public bool IsValid { get; }
    public string? ErrorMessage { get; }

    private ValidationResult(bool isValid, string? errorMessage = null)
    {
        IsValid = isValid;
        ErrorMessage = errorMessage;
    }

    /// <summary>
    /// Tworzy wynik pozytywnej walidacji
    /// </summary>
    public static ValidationResult Success()
    {
        return new ValidationResult(true);
    }

    /// <summary>
    /// Tworzy wynik negatywnej walidacji z komunikatem błędu
    /// </summary>
    public static ValidationResult Failure(string errorMessage)
    {
        return new ValidationResult(false, errorMessage);
    }
}
