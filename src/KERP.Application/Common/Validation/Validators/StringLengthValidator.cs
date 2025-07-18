using KERP.Application.Abstractions.Validation.Chain;

namespace KERP.Application.Common.Validation.Validators;

/// <summary>
/// Handler walidujący długość stringa.
/// Może sprawdzać dokładną długość lub zakres (min/max).
/// </summary>
public class StringLengthValidator : BaseChainValidator<string>
{
    private readonly int? _exactLength;
    private readonly int? _minLength;
    private readonly int? _maxLength;

    /// <summary>
    /// Konstruktor dla dokładnej długości
    /// </summary>
    /// <param name="exactLength">Wymagana dokładna długość</param>
    public StringLengthValidator(int exactLength)
    {
        _exactLength = exactLength;
    }

    /// <summary>
    /// Konstruktor dla zakresu długości
    /// </summary>
    /// <param name="minLength">Minimalna długość (null = brak limitu)</param>
    /// <param name="maxLength">Maksymalna długość (null = brak limitu)</param>
    public StringLengthValidator(int? minLength, int? maxLength)
    {
        _minLength = minLength;
        _maxLength = maxLength;
    }

    protected override Task<ValidationResult> ValidateAsync(
        string value,
        string fieldName,
        CancellationToken cancellationToken)
    {
        // Jeśli wartość jest null, pozwól innemu handlerowi to obsłużyć
        if (value == null)
        {
            return Task.FromResult(ValidationResult.Success());
        }

        var length = value.Length;

        // Sprawdzanie dokładnej długości
        if (_exactLength.HasValue)
        {
            if (length != _exactLength.Value)
            {
                return Task.FromResult(
                    ValidationResult.Failure(
                        $"{fieldName} must be exactly {_exactLength.Value} characters long. Current length: {length}."));
            }
        }
        // Sprawdzanie zakresu
        else
        {
            if (_minLength.HasValue && length < _minLength.Value)
            {
                return Task.FromResult(
                    ValidationResult.Failure(
                        $"{fieldName} must be at least {_minLength.Value} characters long. Current length: {length}."));
            }

            if (_maxLength.HasValue && length > _maxLength.Value)
            {
                return Task.FromResult(
                    ValidationResult.Failure(
                        $"{fieldName} must be at most {_maxLength.Value} characters long. Current length: {length}."));
            }
        }

        return Task.FromResult(ValidationResult.Success());
    }
}