using KERP.Application.Abstractions.Validation;

namespace KERP.Application.Shared.Validation.Rules;

/// <summary>
/// Reguła walidacyjna sprawdzająca, czy data nie jest w przeszłości (porównuje tylko część daty).
/// </summary>
public class DateNotInPastRule : IValidationRule<DateTime>
{
    private readonly string _message;

    public DateNotInPastRule(string message = "Nie można ustawić daty z przeszłości.")
    {
        _message = message;
    }

    public Task<string?> ValidateAsync(DateTime value, CancellationToken cancellationToken = default)
    {
        if (value.Date < DateTime.UtcNow.Date)
        {
            return Task.FromResult<string?>(_message);
        }

        return Task.FromResult<string?>(null);
    }
}