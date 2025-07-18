using KERP.Application.Abstractions.Validation;

namespace KERP.Application.Shared.Validation.Rules;

/// <summary>
/// Reguła walidacyjna sprawdzająca, czy ciąg znaków nie jest pusty, null lub nie składa się z białych znaków.
/// </summary>
public class NotEmptyRule : IValidationRule<string>
{
    private readonly string _message;

    public NotEmptyRule(string message = "Wartość nie może być pusta.")
    {
        _message = message;
    }

    public Task<string?> ValidateAsync(string value, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Task.FromResult<string?>(_message);
        }

        return Task.FromResult<string?>(null);
    }
}