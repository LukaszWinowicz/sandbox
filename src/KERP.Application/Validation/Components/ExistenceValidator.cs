using KERP.Application.Abstractions.Messaging;
using KERP.Application.Validation.Context;
using KERP.Application.Validators.Handlers;

namespace KERP.Application.Validation.Components;

/// <summary>
/// A generic validator that checks if an entity with a given key exists
/// by calling an external asynchronous function.
/// Stops its chain if the entity does not exist.
/// </summary>
/// <typeparam name="T">The type of the key to check (e.g., string, int).</typeparam>
public class ExistenceValidator<T> : ValidationHandler
{
    private readonly Func<CommandBase, T> _valueProvider;
    private readonly Func<T, Task<bool>> _existenceCheckFunc;
    private readonly string _fieldName;

    public ExistenceValidator(Func<CommandBase, T> valueProvider, Func<T, Task<bool>> existenceCheckFunc, string fieldName)
    {
        _valueProvider = valueProvider;
        _existenceCheckFunc = existenceCheckFunc;
        _fieldName = fieldName;
    }

    public override async Task ValidateAsync(ValidationRequest request)
    {
        var value = _valueProvider(request.Command);

        // We only validate if the value is not null. A separate NotNullValidator should handle that.
        if (value != null && !await _existenceCheckFunc(value))
        {
            request.Errors.Add($"{_fieldName} '{value}' does not exist.");
            return; // Critical error, stop this chain.
        }

        await PassToNextAsync(request);
    }
}