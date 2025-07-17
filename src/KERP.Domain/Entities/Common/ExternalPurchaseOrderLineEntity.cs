namespace KERP.Domain.Entities.Common;

/// <summary>
/// Reprezentuje pojedynczą linię zamówienia z zewnętrznego systemu (replika z BigQuery).
/// Służy wyłącznie do celów walidacji i szybkiego odczytu. Nie jest to Agregat.
/// </summary>
public class ExternalPurchaseOrderLineEntity
{
    // Klucz złożony, aby zapewnić unikalność każdej linii
    public string PurchaseOrder { get; set; }
    public int LineNumber { get; set; }
    public int Sequence { get; set; }

    // Dodajemy FactoryId, aby wiedzieć, z której tabeli pochodzi rekord
    public int FactoryId { get; set; }
}
