using KERP.Domain.Enums.MassUpdate.PurchaseOrder;

namespace KERP.Application.MassUpdate.PurchaseOrder.Commands;

/// <summary>
/// Używamy tutaj typu 'record' dla zwięzłości. 
/// 'PurchaseOrderUpdateDto' to prosty obiekt transferu danych (DTO), który reprezentuje jeden wiersz wprowadzony przez użytkownika.
/// </summary>
public record PurchaseOrderReceiptDateUpdateCommand(
    List<PurchaseOrderUpdateDto> OrdersToUpdate
);

public record PurchaseOrderUpdateDto(
    string PurchaseOrder,
    int LineNumber,
    int Sequence,
    DateTime ReceiptDate,
    ReceiptDateUpdateType DateType
);
