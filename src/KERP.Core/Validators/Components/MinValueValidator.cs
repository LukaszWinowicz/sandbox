using KERP.Core.Abstractions.Messaging;
using KERP.Core.Validators.Context;
using KERP.Core.Validators.Handlers;

namespace KERP.Core.Validators.Components;

/// <summary>
/// A generic validator that checks if a value is greater than or equal to a minimum value.
/// It uses IComparable<T> to handle any comparable type (int, decimal, etc.).
/// </summary>
/// <typeparam name="T">The type of the value to check.</typeparam>
public class MinValueValidator<T> : ValidationHandler where T : IComparable<T>
{
    private readonly Func<CommandBase, T> _valueProvider;
    private readonly T _minValue;
    private readonly string _fieldName;

    public MinValueValidator(Func<CommandBase, T> valueProvider, T minValue, string fieldName)
    {
        _valueProvider = valueProvider;
        _minValue = minValue;
        _fieldName = fieldName;
    }

    public override async Task ValidateAsync(ValidationRequest request)
    {
        var value = _valueProvider(request.Command);

        // value.CompareTo(minValue) returns a negative number if 'value' is less than 'minValue'.
        if (value.CompareTo(_minValue) < 0)
        {
            request.Errors.Add($"{_fieldName} must be greater than or equal to {_minValue}.");
        }

        await PassToNextAsync(request);
    }
}