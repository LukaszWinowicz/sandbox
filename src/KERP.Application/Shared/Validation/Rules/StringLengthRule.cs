using KERP.Application.Abstractions.Validation;

namespace KERP.Application.Shared.Validation.Rules;

/// <summary>
/// Reguła walidacyjna sprawdzająca, czy ciąg znaków ma określoną, dokładną długość.
/// </summary>
public class StringLengthRule : IValidationRule<string>
{
    private readonly int _exactLength;
    private readonly string _message;

    public StringLengthRule(int exactLength, string? message = null)
    {
        _exactLength = exactLength;
        _message = message ?? $"Wartość musi mieć dokładnie {_exactLength} znaków.";
    }

    public Task<string?> ValidateAsync(string value, CancellationToken cancellationToken = default)
    {
        if (value?.Length != _exactLength)
        {
            return Task.FromResult<string?>(_message);
        }

        return Task.FromResult<string?>(null);
    }
}