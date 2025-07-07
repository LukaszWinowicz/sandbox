using KERP.Core.Abstractions.Messaging;
using KERP.Core.Validators.Context;
using KERP.Core.Validators.Handlers;

namespace KERP.Core.Validators.Components;

/// <summary>
/// Validates that a string field has a exact, required length.
/// Stops processing for this chain if validation fails.
/// </summary>
public class StringLengthValidator : ValidationHandler
{
    private readonly Func<CommandBase, string?> _valueProvider;
    private readonly int _length;
    private readonly string _fieldName;
    public StringLengthValidator(Func<CommandBase, string?> valueProvider, int length, string fieldName)
    {
        _valueProvider = valueProvider;
        _length = length;
        _fieldName = fieldName;
    }

    public override async Task ValidateAsync(ValidationRequest request)
    {
        var value = _valueProvider(request.Command);

        if (!string.IsNullOrEmpty(value) && value.Length != _length)
        {
            request.Errors.Add($"{_fieldName} must be exactly {_length} characters long.");
            return;
        }

        await PassToNextAsync(request);
    }
}
