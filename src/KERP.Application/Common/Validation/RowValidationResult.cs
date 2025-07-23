namespace KERP.Application.Common.Validation;

/// <summary>
/// Reprezentuje wynik walidacji pojedynczego wiersza danych
/// </summary>
public class RowValidationResult
{
    // <summary>
    /// Numer wiersza (1-based dla czytelności użytkownika)
    /// </summary>
    public int RowNumber { get; }

    /// <summary>
    /// Czy wiersz jest poprawny (brak błędów)
    /// </summary>
    public bool IsValid => !FieldErrors.Any();

    /// <summary>
    /// Słownik błędów: klucz = nazwa pola, wartość = lista błędów dla tego pola
    /// </summary>
    public Dictionary<string, List<string>> FieldErrors { get; }

    private RowValidationResult(int rowNumber)
    {
        RowNumber = rowNumber;
        FieldErrors = new Dictionary<string, List<string>>();
    }

    /// <summary>
    /// Tworzy wynik dla poprawnego wiersza
    /// </summary>
    public static RowValidationResult Success(int rowNumber)
        => new(rowNumber);

    /// <summary>
    /// Tworzy wynik z błędami
    /// </summary>
    public static RowValidationResult WithErrors(int rowNumber, Dictionary<string, List<string>> fieldErrors)
    {
        var result = new RowValidationResult(rowNumber);
        foreach (var error in fieldErrors)
        {
            result.FieldErrors[error.Key] = error.Value;
        }
        return result;
    }

    /// <summary>
    /// Dodaje błąd do konkretnego pola
    /// </summary>
    public void AddFieldError(string fieldName, string errorMessage)
    {
        if (!FieldErrors.ContainsKey(fieldName))
            FieldErrors[fieldName] = new List<string>();

        FieldErrors[fieldName].Add(errorMessage);
    }
}
