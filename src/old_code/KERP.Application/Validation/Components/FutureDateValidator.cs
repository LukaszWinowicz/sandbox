using KERP.Application.Abstractions.Messaging;
using KERP.Application.Validation.Context;
using KERP.Application.Validators.Handlers;

namespace KERP.Application.Validation.Components;
/// <summary>
/// Validates that a given date is today or a future date.
/// It skips validation if the date is null.
/// </summary>
public class FutureDateValidator : ValidationHandler
{
    private readonly Func<CommandBase, DateTime?> _dateProvider;
    private readonly string _fieldName;

    public FutureDateValidator(Func<CommandBase, DateTime?> dateProvider, string fieldName)
    {
        _dateProvider = dateProvider;
        _fieldName = fieldName;
    }

    public override async Task ValidateAsync(ValidationRequest request)
    {
        var dateToValidate = _dateProvider(request.Command);

        // We only validate if a date has been provided.
        // A separate NotNullValidator should handle the case where the date is required but null.
        if (dateToValidate.HasValue && dateToValidate.Value.Date < DateTime.Today)
        {
            request.Errors.Add($"{_fieldName} must be today or a future date.");
        }

        await PassToNextAsync(request);
    }
}