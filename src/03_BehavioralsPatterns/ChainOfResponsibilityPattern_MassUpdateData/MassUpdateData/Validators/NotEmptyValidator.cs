using MassUpdateData.Handlers;
using MassUpdateData.Models;

namespace MassUpdateData.Validators;

public class NotEmptyValidator : ValidationHandler
{
    private readonly Func<MassUpdateDto, string> _valueProvider;
    private readonly string _fieldName;

    public NotEmptyValidator(Func<MassUpdateDto, string> valueProvider, string fieldName)
    {
        _valueProvider = valueProvider;
        _fieldName = fieldName;
    }

    public override void Validate(ValidationRequest request)
    {
        var value = _valueProvider(request.Dto);
        if (string.IsNullOrEmpty(value))
        {
            request.ValidationErrors.Add($"{_fieldName} is required.");
        }
        PassToNext(request);
    }
}
