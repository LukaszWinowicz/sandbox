using KERP.Application.MassUpdate.PurchaseOrder.Commands;

namespace KERP.Application.Shared.Validation.Validators;

/// <summary>
/// Generyczny, reużywalny walidator, który sprawdza, czy data jest dzisiejsza lub przyszła.
/// </summary>
public class FutureDateValidator<T> : AbstractValidationHandler<T> where T : class
{
    private readonly Func<T, DateTime> _dateProvider;
    private readonly string _fieldName;

    /// <summary>
    /// Tworzy instancję walidatora.
    /// </summary>
    /// <param name="dateProvider">Funkcja, która wyciąga datę z obiektu DTO.</param>
    /// <param name="fieldName">Nazwa pola do użycia w komunikacie o błędzie.</param>
    public FutureDateValidator(Func<T, DateTime> dateProvider, string fieldName)
    {
        _dateProvider = dateProvider;
        _fieldName = fieldName;
    }

    protected override Task HandleValidation(ValidationRequest<T> request)
    {
        var dateToValidate = _dateProvider(request.DtoToValidate);

        if (dateToValidate.Date < DateTime.UtcNow.Date)
        {
            request.Errors.Add($"{_fieldName} must be today or a future date.");
        }

        return Task.CompletedTask;
    }
}