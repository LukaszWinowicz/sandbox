using KERP.Application.Common.Abstractions;
using KERP.Application.Common.Models;
using KERP.Application.Common.Validation;
using KERP.Domain.Aggregates.PurchaseOrder;

namespace KERP.Application.Features.MassUpdates.PurchaseOrder.UpdateReceiptDate;


// Zwracamy prosty Result, który sygnalizuje sukces lub porażkę z listą błędów.
public sealed class RequestPurchaseOrderReceiptDateChangeCommand : ICommand<Result<List<RowValidationResult>>>
{
    // Lista linii zamówień do zaktualizowania
    public List<OrderLineDto> OrderLines { get; init; }

    // Typ daty wspólny dla wszystkich aktualizowanych linii
    public ReceiptDateUpdateType DateType { get; init; }
}