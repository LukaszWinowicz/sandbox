using MassUpdateData.Handlers;
using MassUpdateData.Models;

namespace MassUpdateData.Validators;

public class OrderFormatValidator : ValidationHandler
{
    private readonly int _requiredLength;
    private readonly string _errorMessageTemplate;

    public OrderFormatValidator(int requiredLength, string errorMessageTemplate)
    {
        _requiredLength = requiredLength;
        _errorMessageTemplate = errorMessageTemplate;
    }

    public override void Validate(UpdateRequest request)
    {
        if (request.Order?.Length != _requiredLength)
        {
            // Używamy skonfigurowanego komunikatu
            request.ValidationErrors.Add(string.Format(_errorMessageTemplate, _requiredLength));
        }
        // Niezależnie od wyniku, przekazujemy dalej, by zebrać wszystkie błędy
        PassToNext(request);
    }
}
