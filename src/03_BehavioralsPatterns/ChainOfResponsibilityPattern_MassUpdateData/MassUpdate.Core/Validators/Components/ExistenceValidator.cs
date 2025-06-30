using MassUpdate.Core.DTOs;
using MassUpdate.Core.Handlers;

namespace MassUpdate.Core.Validators.Components;

public class ExistenceValidator<T> : ValidationHandler
{
    private readonly Func<MassUpdateDto, T> _valueProvider;
    private readonly Func<T, Task<bool>> _existenceCheckFunc; // np. _service.OrderExists
    private readonly string _fieldName;

    public ExistenceValidator(Func<MassUpdateDto, T> valueProvider, Func<T, Task<bool>> existenceCheckFunc, string fieldName)
    {
        _valueProvider = valueProvider;
        _existenceCheckFunc = existenceCheckFunc;
        _fieldName = fieldName;
    }

    public override async Task ValidateAsync(ValidationRequest request)
    {
        var value = _valueProvider(request.Dto);
        if (value == null || !await _existenceCheckFunc(value)) // Używamy await
        {
            request.ValidationErrors.Add($"{_fieldName} '{value}' does not exist.");
            return;
        }
        await PassToNextAsync(request);
    }
}
