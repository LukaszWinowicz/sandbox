using KERP.Application.Abstractions.Validation.Chain;

namespace KERP.Application.Common.Validation.Validators;

/// <summary>
/// Walidator sprawdzający czy wartość istnieje w bazie danych.
/// Używa delegata do sprawdzenia istnienia.
/// </summary>
public class ExistsInDatabaseValidator : BaseChainValidator<string>
{
    private readonly Func<string, CancellationToken, Task<bool>> _existsCheck;
    private readonly string _entityDisplayName;

    /// <summary>
    /// Tworzy walidator sprawdzający istnienie
    /// </summary>
    /// <param name="existsCheck">Funkcja sprawdzająca czy wartość istnieje</param>
    /// <param name="entityDisplayName">Nazwa encji do wyświetlenia w błędzie</param>
    public ExistsInDatabaseValidator(
        Func<string, CancellationToken, Task<bool>> existsCheck,
        string entityDisplayName)
    {
        _existsCheck = existsCheck ?? throw new ArgumentNullException(nameof(existsCheck));
        _entityDisplayName = entityDisplayName ?? throw new ArgumentNullException(nameof(entityDisplayName));
    }

    protected override async Task<ValidationResult> ValidateAsync(
        string value,
        string fieldName,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return ValidationResult.Success();
        }

        try
        {
            var exists = await _existsCheck(value, cancellationToken);

            if (!exists)
            {
                return ValidationResult.Failure(
                    $"{fieldName} '{value}' does not exist in {_entityDisplayName}.");
            }

            return ValidationResult.Success();
        }
        catch (Exception ex)
        {
            return ValidationResult.Failure(
                $"Unable to verify {fieldName} '{value}'. Please try again later.");
        }
    }
}