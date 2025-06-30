using MassUpdate.Core.DTOs;
using MassUpdate.Core.Handlers;

namespace MassUpdate.Core.Validators.Components;

public class NotNullValidator<T> : ValidationHandler
{
    private readonly Func<MassUpdateDto, T?> _valueProvider;
    private readonly string _fieldName;
    public NotNullValidator(Func<MassUpdateDto, T?> valueProvider, string fieldName) 
    {
        _valueProvider = valueProvider;
        _fieldName = fieldName;
    }

    public override async Task ValidateAsync(ValidationRequest request)
    {
        if (_valueProvider(request.Dto) == null)
        {
            request.ValidationErrors.Add($"{_fieldName} is required.");
            return;
        }
        await PassToNextAsync(request);
    }
}
