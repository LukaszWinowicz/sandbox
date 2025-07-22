namespace KERP.Application.Features.MassUpdates.PurchaseOrder.UpdateReceiptDate;

public sealed record OrderLineDto(
string PurchaseOrder,
int LineNumber,
int Sequence,
DateTime NewReceiptDate);