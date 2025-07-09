namespace KERP.Application.Validation.Components;
/// <summary>
/// Validates that a given value is not null.
/// This is a generic validator that works for both nullable value types (e.g., int?, DateTime?)
/// and reference types. It stops processing for its chain if validation fails.
/// </summary>
/// <typeparam name="T">The type of the value to check.</typeparam>
public class NotNullValidator<T> : ValidationHandler
{
    private readonly Func<CommandBase, T?> _valueProvider;
    private readonly string _fieldName;

    public NotNullValidator(Func<CommandBase, T?> valueProvider, string fieldName)
    {
        _valueProvider = valueProvider;
        _fieldName = fieldName;
    }

    public override async Task ValidateAsync(ValidationRequest request)
    {
        var value = _valueProvider(request.Command);

        if (value == null)
        {
            request.Errors.Add($"{_fieldName} is required.");
            return;
        }

        await PassToNextAsync(request);
    }
}
