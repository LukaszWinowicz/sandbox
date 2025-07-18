using KERP.Application.Abstractions.Queries.Repositories;
using KERP.Application.Abstractions.Validation;
using KERP.Application.Features.MassUpdate.PurchaseOrder.Commands.RequestUpdateReceiptDate;
using KERP.Application.Shared.Validation;
using KERP.Domain.Enums;

namespace KERP.Application.Features.MassUpdate.PurchaseOrder.Validation;

public class PurchaseOrderReceiptDateUpdateValidator : IValidator<RequestPurchaseOrderReceiptDateUpdateCommand>
{
    private readonly IExternalPurchaseOrderRepository _repo;

    // Definiujemy "skompilowane" funkcje walidacyjne raz, aby nie tworzyć ich w pętli
    private readonly Func<string, CancellationToken, Task<string?>> _purchaseOrderValidator;
    private readonly Func<int, CancellationToken, Task<string?>> _lineNumberValidator;
    private readonly Func<int, CancellationToken, Task<string?>> _sequenceValidator;
    private readonly Func<DateTime, CancellationToken, Task<string?>> _receiptDateValidator;

    public PurchaseOrderReceiptDateUpdateValidator(IExternalPurchaseOrderRepository repo)
    {
        _repo = repo;

        // Budujemy łańcuchy walidacji w konstruktorze
        _purchaseOrderValidator = new ValidationBuilder<string>()
            .NotEmpty("Numer zamówienia nie może być pusty.")
            .HasExactLength(10, "Numer zamówienia musi mieć dokładnie 10 znaków.")
            .MustExistInRepo(_repo) // Używamy naszej nowej metody rozszerzającej
            .Build();

        _lineNumberValidator = new ValidationBuilder<int>()
            .IsGreaterThanOrEqualTo(10, "Numer linii musi być większy lub równy 10.")
            .Build();

        _sequenceValidator = new ValidationBuilder<int>()
            .IsGreaterThanOrEqualTo(1, "Sekwencja musi być większa lub równa 1.")
            .Build();

        _receiptDateValidator = new ValidationBuilder<DateTime>()
            .IsNotInPast("Nie można ustawić daty z przeszłości.")
            .Build();
    }

    public async Task<IReadOnlyCollection<string>> ValidateAsync(RequestPurchaseOrderReceiptDateUpdateCommand command, CancellationToken cancellationToken = default)
    {
        var errors = new List<string>();

        foreach (var (line, index) in command.OrderLines.Select((line, index) => (line, index)))
        {
            var errorPrefix = $"Błąd w wierszu {index + 1}:";

            // Walidacja numeru zamówienia
            var poError = await _purchaseOrderValidator(line.PurchaseOrder, cancellationToken);
            if (poError is not null) errors.Add($"{errorPrefix} {poError}");

            // Walidacja numeru linii
            var lineError = await _lineNumberValidator(line.LineNumber, cancellationToken);
            if (lineError is not null) errors.Add($"{errorPrefix} {lineError}");

            // Walidacja sekwencji
            var seqError = await _sequenceValidator(line.Sequence, cancellationToken);
            if (seqError is not null) errors.Add($"{errorPrefix} {seqError}");

            // Walidacja nowej daty
            var dateError = await _receiptDateValidator(line.NewReceiptDate, cancellationToken);
            if (dateError is not null) errors.Add($"{errorPrefix} {dateError}");
        }

        // Walidacja typu daty (prosta walidacja, nie wymagała dedykowanej reguły)
        if (!Enum.IsDefined(typeof(ReceiptDateUpdateType), command.DateType))
        {
            errors.Add("Nieprawidłowy typ aktualizacji daty.");
        }

        return errors;
    }
}