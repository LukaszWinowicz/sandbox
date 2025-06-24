using MassUpdate.Core.DTOs;
using MassUpdate.Core.Handlers;

namespace MassUpdate.Core.Validators.Components;

public class StringLengthValidator : ValidationHandler
{
    private readonly Func<MassUpdateDto, string> _valueProvider;
    private readonly int _length;
    private readonly string _fieldName;

    public StringLengthValidator(Func<MassUpdateDto, string> valueProvider, int length, string fieldName)
    {
        _valueProvider = valueProvider;
        _length = length;
        _fieldName = fieldName;
    }

    public override void Validate(ValidationRequest request)
    {
        var value = _valueProvider(request.Dto);
        if (!string.IsNullOrEmpty(value) && value.Length != _length)
        {
            request.ValidationErrors.Add($"{_fieldName} must be exactly {_length} characters long.");
            // Błąd formatu jest krytyczny dla dalszych sprawdzeń tego pola, więc zatrzymujemy łańcuch.
            return;

        }
        PassToNext(request);
    }
}
