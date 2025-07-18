using KERP.Application.Abstractions.Validation.Chain;

namespace KERP.Application.Common.Validation.Handlers;

/// <summary>
/// Handler walidujący czy wartość nie jest pusta.
/// Dla string: sprawdza null, empty i whitespace
/// Dla innych typów: sprawdza tylko null
/// </summary>
public class NotEmptyValidator : BaseValidationHandler<string>
{
    protected override Task<ValidationResult> ValidateAsync(
            string value,
            string fieldName,
            CancellationToken cancellationToken)
    {
        // Sprawdzamy czy string jest null, pusty lub zawiera tylko białe znaki
        if (string.IsNullOrWhiteSpace(value))
        {
            return Task.FromResult(
                ValidationResult.Failure($"{fieldName} cannot be empty."));
        }

        // Walidacja przeszła pomyślnie
        return Task.FromResult(ValidationResult.Success());
    }
}
