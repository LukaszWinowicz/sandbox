using KERP.Application.Abstractions.Messaging;
using KERP.Application.Validation.Context;
using KERP.Application.Validators.Handlers;

namespace KERP.Application.Validation.Components;
/// <summary>
/// Validates that a string field is not null or empty.
/// Stops processing for this chain if validation fails.
/// </summary>
public class NotEmptyValidator : ValidationHandler
{
    private readonly Func<CommandBase, string?> _valueProvider;
    private readonly string _fieldName;

    public NotEmptyValidator(Func<CommandBase, string?> valueProvider, string fieldName)
    {
        _valueProvider = valueProvider;
        _fieldName = fieldName;
    }

    public override async Task ValidateAsync(ValidationRequest request)
    {
        var value = _valueProvider(request.Command);
        if (string.IsNullOrEmpty(value))
        {
            request.Errors.Add($"{_fieldName} is required.");
            return;
        }
        await PassToNextAsync(request);
    }
}
