namespace KERP.Infrastructure.Data.ReadModels;

/// <summary>
/// Reprezentuje pojedynczą linię zamówienia z zewnętrznego systemu (replika z BigQuery).
/// Służy wyłącznie do celów walidacji i szybkiego odczytu. Nie jest to Agregat.
/// </summary>
public class ExternalPurchaseOrderLineReadModel
{
    // Klucz złożony, aby zapewnić unikalność każdej linii
    public string PurchaseOrder { get; set; }
    public int LineNumber { get; set; }
    public int Sequence { get; set; }
}
