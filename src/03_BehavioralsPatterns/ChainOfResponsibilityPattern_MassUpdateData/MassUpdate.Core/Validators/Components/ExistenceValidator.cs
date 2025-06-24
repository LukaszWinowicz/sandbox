using MassUpdate.Core.DTOs;
using MassUpdate.Core.Handlers;

namespace MassUpdate.Core.Validators.Components;

public class ExistenceValidator<T> : ValidationHandler
{
    private readonly Func<MassUpdateDto, T> _valueProvider;
    private readonly Func<T, bool> _existenceCheckFunc; // np. _service.OrderExists
    private readonly string _fieldName;

    public ExistenceValidator(Func<MassUpdateDto, T> valueProvider, Func<T, bool> existenceCheckFunc, string fieldName)
    {
        _valueProvider = valueProvider;
        _existenceCheckFunc = existenceCheckFunc;
        _fieldName = fieldName;
    }

    public override void Validate(ValidationRequest request)
    {
        var value = _valueProvider(request.Dto);

        if (value == null || !_existenceCheckFunc(value))
        {
            request.ValidationErrors.Add($"{_fieldName} '{value}' does not exist.");
            return; // ZATRZYMANIE ŁAŃCUCHA
        }
        PassToNext(request);
    }
}
