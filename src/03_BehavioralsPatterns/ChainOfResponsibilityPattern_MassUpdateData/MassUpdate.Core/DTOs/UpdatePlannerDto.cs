namespace MassUpdate.Core.DTOs;

public class UpdatePlannerDto : MassUpdateDto
{
    public required string PurchaseOrder {  get; set; }
    public required string PlannerId { get; set; }
}
