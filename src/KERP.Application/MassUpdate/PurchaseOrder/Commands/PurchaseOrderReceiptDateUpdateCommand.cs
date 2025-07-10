using KERP.Domain.Enums.MassUpdate.PurchaseOrder;

namespace KERP.Application.MassUpdate.PurchaseOrder.Commands;

/// <summary>
/// Używamy tutaj typu 'record' dla zwięzłości. 
/// 'PurchaseOrderUpdateDto' to prosty obiekt transferu danych (DTO), który reprezentuje jeden wiersz wprowadzony przez użytkownika.
/// </summary>
public record PurchaseOrderReceiptDateUpdateCommand(
    List<PurchaseOrderUpdateDto> OrdersToUpdate
);

// DTO zamieniamy na zwykłą klasę, aby umożliwić bindowanie w Blazorze
public class PurchaseOrderUpdateDto
{
    public string PurchaseOrder { get; set; } = string.Empty;
    public int LineNumber { get; set; }
    public int Sequence { get; set; }
    public DateTime ReceiptDate { get; set; }
    public ReceiptDateUpdateType DateType { get; set; }

    // Dodajemy pusty konstruktor, którego może wymagać Blazor
    public PurchaseOrderUpdateDto() { }

    // Opcjonalny konstruktor do łatwiejszego tworzenia obiektu w kodzie
    public PurchaseOrderUpdateDto(string purchaseOrder, int lineNumber, int sequence, DateTime receiptDate, ReceiptDateUpdateType dateType)
    {
        PurchaseOrder = purchaseOrder;
        LineNumber = lineNumber;
        Sequence = sequence;
        ReceiptDate = receiptDate;
        DateType = dateType;
    }
}
