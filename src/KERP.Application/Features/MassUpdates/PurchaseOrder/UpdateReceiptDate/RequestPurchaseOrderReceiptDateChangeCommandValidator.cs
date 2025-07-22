using KERP.Application.Validation;

namespace KERP.Application.Features.MassUpdates.PurchaseOrder.UpdateReceiptDate;

public class RequestPurchaseOrderReceiptDateChangeCommandValidator
    : IValidator<RequestPurchaseOrderReceiptDateChangeCommand>
{
    public ValidationResult Validate(RequestPurchaseOrderReceiptDateChangeCommand command)
    {
        var errors = new List<ValidationError>();

        if (command.OrderLines is null || !command.OrderLines.Any())
        {
            errors.Add(new ValidationError(
                nameof(command.OrderLines),
                "Lista linii zamówień nie może być pusta."));

            // Jeśli lista jest pusta, nie ma sensu walidować dalej
            return new ValidationResult(errors);
        }

        foreach (var line in command.OrderLines)
        {
            if (string.IsNullOrWhiteSpace(line.PurchaseOrder))
            {
                errors.Add(new ValidationError(
                    nameof(line.PurchaseOrder),
                    "Numer zamówienia (PurchaseOrder) jest wymagany dla każdej linii."));
            }

            if (line.LineNumber <= 0)
            {
                errors.Add(new ValidationError(
                    nameof(line.LineNumber),
                    "Numer linii (LineNumber) musi być większy od zera."));
            }
        }

        return new ValidationResult(errors);
    }
}
