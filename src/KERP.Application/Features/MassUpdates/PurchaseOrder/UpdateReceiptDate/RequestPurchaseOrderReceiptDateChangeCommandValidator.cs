using KERP.Application.Validation;

namespace KERP.Application.Features.MassUpdates.PurchaseOrder.UpdateReceiptDate;

public class RequestPurchaseOrderReceiptDateChangeCommandValidator
    : IValidator<RequestPurchaseOrderReceiptDateChangeCommand>
{
    public ValidationResult Validate(RequestPurchaseOrderReceiptDateChangeCommand command)
    {
        var errors = new List<ValidationError>();

        foreach (var line in command.OrderLines)
        {
            // Purchase Order
            if (string.IsNullOrWhiteSpace(line.PurchaseOrder))
            {
                errors.Add(new ValidationError(
                    nameof(line.PurchaseOrder),
                    "Numer zamówienia (PurchaseOrder) jest wymagany."));
            }

            if (line.PurchaseOrder.Length != 10)
            {
                errors.Add(new ValidationError(
                    nameof(line.PurchaseOrder),
                    "Numer zamówienia (PurchaseOrder) musi mieć dokładnie 10 znaków."));
            }


            // LineNumber
            if (line.LineNumber.CompareTo(10) < 0)
            {
                errors.Add(new ValidationError(
                    nameof(line.LineNumber),
                    "Line Number zamówienia musi być większy lub równy 10."));
            }

            // Sequence
            if (line.LineNumber.CompareTo(1) < 0)
            {
                errors.Add(new ValidationError(
                    nameof(line.Sequence),
                    "Line Number zamówienia musi być większy lub równy 1."));
            }


            // ReceiptDate  
            if (line.NewReceiptDate.Date < DateTime.Today)
            {
                errors.Add(new ValidationError(
                    nameof(line.NewReceiptDate),
                    "Receipt Date musi być z przyszłości."));
            }
        }

        return new ValidationResult(errors);
    }
}
