namespace KERP.Application.Common.Validation.Validators;

/// <summary>
/// Wynik walidacji dla operacji masowych (batch).
/// Przechowuje szczegółowe informacje o błędach per wiersz.
/// </summary>
public class BatchValidationResult : ValidationResult
{
    /// <summary>
    /// Wyniki walidacji dla poszczególnych wierszy.
    /// Zawiera zarówno wiersze z błędami jak i poprawne.
    /// </summary>
    public IReadOnlyList<RowValidationResult> RowResults { get; }

    /// <summary>
    /// Liczba wszystkich przetworzonych wierszy.
    /// </summary>
    public int TotalRows => RowResults.Count;

    /// <summary>
    /// Liczba wierszy które przeszły walidację.
    /// </summary>
    public int ValidRows => RowResults.Count(r => r.IsValid);

    /// <summary>
    /// Liczba wierszy z błędami.
    /// </summary>
    public int InvalidRows => RowResults.Count(r => !r.IsValid);

    /// <summary>
    /// Tworzy nowy wynik walidacji batch.
    /// </summary>
    /// <param name="rowResults">Wyniki walidacji poszczególnych wierszy</param>
    public BatchValidationResult(List<RowValidationResult> rowResults)
        : base(ConvertToFlatErrors(rowResults))
    {
        RowResults = rowResults.AsReadOnly();
    }

    /// <summary>
    /// Konwertuje błędy z wierszy na płaską listę ValidationError.
    /// Format: "Row[{rowNumber}].{fieldName}" jako PropertyName.
    /// </summary>
    private static List<ValidationError> ConvertToFlatErrors(List<RowValidationResult> rowResults)
    {
        var errors = new List<ValidationError>();

        foreach (var row in rowResults.Where(r => !r.IsValid))
        {
            foreach (var fieldError in row.FieldErrors)
            {
                var fieldName = fieldError.Key;
                var fieldErrors = fieldError.Value;

                foreach (var errorMessage in fieldErrors)
                {
                    errors.Add(new ValidationError(
                        PropertyName: $"Row[{row.RowNumber}].{fieldName}",
                        ErrorMessage: errorMessage
                    ));
                }
            }
        }

        return errors;
    }

    /// <summary>
    /// Pobiera błędy dla konkretnego wiersza.
    /// </summary>
    /// <param name="rowNumber">Numer wiersza (1-based)</param>
    /// <returns>Wynik walidacji dla wiersza lub null jeśli wiersz nie istnieje</returns>
    public RowValidationResult? GetRowResult(int rowNumber)
    {
        return RowResults.FirstOrDefault(r => r.RowNumber == rowNumber);
    }

    /// <summary>
    /// Pobiera wszystkie wiersze z błędami.
    /// </summary>
    /// <returns>Lista wierszy które nie przeszły walidacji</returns>
    public IEnumerable<RowValidationResult> GetInvalidRows()
    {
        return RowResults.Where(r => !r.IsValid);
    }

    /// <summary>
    /// Generuje podsumowanie błędów pogrupowane po typie błędu.
    /// Przydatne do wyświetlania statystyk.
    /// </summary>
    /// <returns>Słownik gdzie klucz to treść błędu, wartość to liczba wystąpień</returns>
    public Dictionary<string, int> GetErrorSummary()
    {
        return Errors
            .GroupBy(e => e.ErrorMessage)
            .ToDictionary(g => g.Key, g => g.Count());
    }
}