namespace KERP.Domain.Aggregates.TempPurchaseOrder;

public class ExternalPurchaseOrder
{
    public int Id { get; set; }
    public string PurchaseOrder { get; }
    public int LineNumber { get; }
    public int Sequence { get; }
}
