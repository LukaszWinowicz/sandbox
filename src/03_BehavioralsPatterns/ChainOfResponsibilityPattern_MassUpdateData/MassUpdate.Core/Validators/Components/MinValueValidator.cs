using MassUpdate.Core.DTOs;
using MassUpdate.Core.Handlers;

namespace MassUpdate.Core.Validators.Components;

// Generyczny walidator wartości minimalnej, działa dla int, decimal, itp.
public class MinValueValidator<T> : ValidationHandler where T : IComparable<T>
{
    private readonly Func<MassUpdateDto, T> _valueProvider;
    private readonly T _minValue;
    private readonly string _fieldName;

    public MinValueValidator(Func<MassUpdateDto, T> valueProvider, T minValue, string fieldName)
    {
        _valueProvider = valueProvider;
        _minValue = minValue;
        _fieldName = fieldName;
    }

    public override void Validate(ValidationRequest request)
    {
        var value = _valueProvider(request.Dto);
        // CompareTo zwraca < 0, jeśli wartość jest mniejsza od minValue
        if (value.CompareTo(_minValue) < 0)
        {
            request.ValidationErrors.Add($"{_fieldName} must be greater than or equal to {_minValue}.");
        }
        PassToNext(request);
    }
}
