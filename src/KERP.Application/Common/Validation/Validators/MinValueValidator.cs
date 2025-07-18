using KERP.Application.Abstractions.Validation.Chain;

namespace KERP.Application.Common.Validation.Validators;

// <summary>
/// Walidator sprawdzający czy wartość jest większa lub równa od minimalnej.
/// Działa dla typów implementujących IComparable (int, decimal, DateTime, etc.)
/// </summary>
/// <typeparam name="T">Typ wartości implementujący IComparable</typeparam>
public class MinValueValidator<T> : BaseChainValidator<T>
    where T : IComparable<T>
{
    private readonly T _minValue;
    private readonly bool _inclusive;

    /// <summary>
    /// Tworzy walidator wartości minimalnej
    /// </summary>
    /// <param name="minValue">Minimalna dozwolona wartość</param>
    /// <param name="inclusive">Czy wartość minimalna jest dozwolona (domyślnie: true)</param>
    public MinValueValidator(T minValue, bool inclusive = true)
    {
        _minValue = minValue;
        _inclusive = inclusive;
    }

    protected override Task<ValidationResult> ValidateAsync(
        T value,
        string fieldName,
        CancellationToken cancellationToken)
    {
        // Null check - pozwalamy innym walidatorom to obsłużyć
        if (value == null)
        {
            return Task.FromResult(ValidationResult.Success());
        }

        var comparisonResult = value.CompareTo(_minValue);

        // Sprawdzenie czy wartość spełnia warunek
        var isValid = _inclusive ? comparisonResult >= 0 : comparisonResult > 0;

        if (!isValid)
        {
            var comparisonText = _inclusive ? "greater than or equal to" : "greater than";
            return Task.FromResult(
                ValidationResult.Failure(
                    $"{fieldName} must be {comparisonText} {_minValue}. Current value: {value}."));
        }

        return Task.FromResult(ValidationResult.Success());
    }
}