using MassUpdate.Core.Enums;

namespace MassUpdate.Core.DTOs;

public class UpdateReceiptDateDto : MassUpdateDto
{
    public required string PurchaseOrder { get; set; }
    public required int LineNumber { get; set; }
    public required int Sequence { get; set; }
    public required DateTime? ReceiptDate { get; set; }
    public required ReceiptDateUpdateType DateType { get; set; }
}
