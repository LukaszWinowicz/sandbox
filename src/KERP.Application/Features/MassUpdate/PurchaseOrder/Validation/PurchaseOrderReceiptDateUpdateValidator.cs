using KERP.Application.Abstractions.Queries.Repositories;
using KERP.Application.Abstractions.Validation;
using KERP.Application.Features.MassUpdate.PurchaseOrder.Commands.RequestUpdateReceiptDate;
using KERP.Domain.Enums;

namespace KERP.Application.Features.MassUpdate.PurchaseOrder.Validation;

public class PurchaseOrderReceiptDateUpdateValidator : IValidator<RequestPurchaseOrderReceiptDateUpdateCommand>
{
    private readonly IExternalPurchaseOrderRepository _repo;

    public PurchaseOrderReceiptDateUpdateValidator(IExternalPurchaseOrderRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<string>> ValidateAsync(RequestPurchaseOrderReceiptDateUpdateCommand command, CancellationToken cancellationToken = default)
    {
        var errors = new List<string>();

        foreach (var line in command.OrderLines)
        {
            if (string.IsNullOrWhiteSpace(line.PurchaseOrder))
                errors.Add("Numer zamówienia nie może być pusty.");

            if (line.PurchaseOrder?.Length != 10)
                errors.Add("Numer zamówienia musi mieć dokładnie 10 znaków.");

            if (!await _repo.ExistsAsync(line.PurchaseOrder, CancellationToken.None))
                errors.Add($"Zamówienie {line.PurchaseOrder} nie istnieje.");

            if (line.LineNumber < 10)
                errors.Add("Numer linii musi być większy lub równy 10.");

            if (line.Sequence < 1)
                errors.Add("Sekwencja musi być większa lub równa 1.");

            //if (!await _repo.ExistsOrderLineAsync(line.PurchaseOrder, line.LineNumber, line.Sequence))
            //    errors.Add($"Nie znaleziono linii {line.LineNumber} i sekwencji {line.Sequence} dla zamówienia {line.PurchaseOrder}.");

            if (line.NewReceiptDate < DateTime.UtcNow.Date)
                errors.Add("Nie można ustawić daty z przeszłości.");

            if (!Enum.IsDefined(typeof(ReceiptDateUpdateType), command.DateType))
                errors.Add("Nieprawidłowy typ aktualizacji daty.");
        }

        return errors;
    }
}