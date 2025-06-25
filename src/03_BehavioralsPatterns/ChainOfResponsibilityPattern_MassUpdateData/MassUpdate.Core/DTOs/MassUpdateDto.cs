namespace MassUpdate.Core.DTOs;

// Klasa bazowa, aby mieć wspólny typ
public abstract class MassUpdateDto { }

public class MassUpdatePurchaseOrderDto : MassUpdateDto
{
    public required string PurchaseOrder { get; set; }
    public int LineNumber { get; set; }
    public int Sequence { get; set; }
    public DateTime? ReceiptDate { get; set; }
}

public class MassUpdateProductionOrderDto : MassUpdateDto
{
    public required string ProductionOrder { get; set; }
    public DateTime? NewDate { get; set; }
    public required string UpdateType { get; set; }
}

public class MassUpdateProductionPlannerDto : MassUpdateDto
{
    public required string ProductionOrder { get; set; }
    public required string Planner {  get; set; }
}