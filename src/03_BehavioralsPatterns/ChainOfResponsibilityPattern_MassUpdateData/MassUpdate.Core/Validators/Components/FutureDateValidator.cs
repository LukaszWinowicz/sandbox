using MassUpdate.Core.DTOs;
using MassUpdate.Core.Handlers;

namespace MassUpdate.Core.Validators.Components;

public class FutureDateValidator : ValidationHandler
{
    private readonly Func<MassUpdateDto, DateTime?> _dateProvider;
    private readonly string _fieldName;

    public FutureDateValidator(Func<MassUpdateDto, DateTime?> dateProvider, string fieldName)
    {
        _dateProvider = dateProvider;
        _fieldName = fieldName;
    }

    public override void Validate(ValidationRequest request)
    {
        // Używamy "sposobu", który dostaliśmy, aby pobrać datę
        var dateToValidate = _dateProvider(request.Dto);

        // Najpierw sprawdzamy, czy data w ogóle została podana.
        // Walidacja "NotEmpty" powinna to obsłużyć, ale to jest dodatkowe zabezpieczenie.
        if (dateToValidate == null)
        {
            // Można tu dodać błąd lub po prostu przejść dalej,
            // zakładając, że NotEmptyValidator już zgłosił problem.
            // Na razie przechodzimy dalej.
            request.ValidationErrors.Add($"{_fieldName} is required.");
        }
        // Jeśli data istnieje, sprawdzamy, czy jest z przeszłości.
        else if (dateToValidate.Value.Date < DateTime.Today)
        {
            // Używamy nazwy pola do zbudowania komunikatu
            request.ValidationErrors.Add($"{_fieldName} must be today or a future date.");
        }

        PassToNext(request);
    }
}