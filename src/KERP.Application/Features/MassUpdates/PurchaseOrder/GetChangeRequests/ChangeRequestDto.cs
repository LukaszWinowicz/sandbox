namespace KERP.Application.Features.MassUpdates.PurchaseOrder.GetChangeRequests;

public class ChangeRequestDto
{
    public int Id { get; set; }
    public string PurchaseOrder { get; set; }
    public int LineNumber { get; set; }
    public DateTime NewReceiptDate { get; set; }
    public string DateType { get; set; }
    public string UserId { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public bool IsProcessed { get; set; }
}