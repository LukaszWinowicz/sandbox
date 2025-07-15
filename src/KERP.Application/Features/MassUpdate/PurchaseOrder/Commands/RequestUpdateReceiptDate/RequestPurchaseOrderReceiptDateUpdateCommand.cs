using KERP.Application.Abstractions.CQRS;
using KERP.Application.Features.MassUpdate.PurchaseOrder.Commands.RequestUpdateReceiptDate.DTOs;
using KERP.Domain.Enums;

namespace KERP.Application.Features.MassUpdate.PurchaseOrder.Commands.RequestUpdateReceiptDate;

/// <summary>
/// Komenda reprezentująca żądanie aktualizacji dat odbioru dla wielu linii zamówień.
/// </summary>
public sealed class RequestPurchaseOrderReceiptDateUpdateCommand : ICommand
{
    public List<OrderLineDto> OrderLines { get; init; }
    public ReceiptDateUpdateType DateType { get; init; }
}
