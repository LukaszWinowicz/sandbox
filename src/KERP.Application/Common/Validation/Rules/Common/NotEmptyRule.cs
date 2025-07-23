namespace KERP.Application.Common.Validation.Rules.Common;

/// <summary>
/// Reguła sprawdzająca czy string nie jest pusty ani nie składa się tylko z białych znaków.
/// </summary>
public class NotEmptyRule : IValidationRule<string>
{
    /// <summary>
    /// Komunikat błędu wyświetlany gdy pole jest puste.
    /// </summary>
    public string ErrorMessage { get; }

    /// <summary>
    /// Tworzy nową instancję reguły NotEmpty.
    /// </summary>
    /// <param name="errorMessage">Opcjonalny własny komunikat błędu. 
    /// Jeśli nie podany, używany jest domyślny.</param>
    public NotEmptyRule(string? errorMessage = null)
    {
        ErrorMessage = errorMessage ?? "Pole jest wymagane";
    }

    /// <summary>
    /// Sprawdza czy wartość nie jest pusta.
    /// </summary>
    /// <param name="value">String do sprawdzenia</param>
    /// <returns>False jeśli wartość jest null, pusta lub składa się tylko z białych znaków</returns>
    public bool Validate(string value)
    {
        return !string.IsNullOrWhiteSpace(value);
    }
}
