using KERP.Domain.Entities.Shared;
using KERP.Domain.Enums.MassUpdate.PurchaseOrder;

namespace KERP.Domain.Entities.MassUpdate.PurchaseOrder;

/// <summary>
/// Reprezentuje pojedynczy, utrwalony rekord żądania aktualizacji daty odbioru dla zamówienia zakupu.
/// </summary>
public class PurchaseOrderReceiptDateUpdateEntity
{
    public int Id { get; set; }
    public required string PurchaseOrder { get; set; }
    public required int LineNumber { get; set; }
    public required int Sequence { get; set; }
    public required DateTime ReceiptDate { get; set; }
    public required ReceiptDateUpdateType DateType { get; set; }

    /// <summary>
    /// Klucz obcy do encji Factory.
    /// </summary>
    public required int FactoryId { get; set; }

    /// <summary>
    /// Właściwość nawigacyjna do powiązanej fabryki.
    /// To pole już NIE JEST wymagane w momencie tworzenia obiektu.
    /// </summary>
    public required FactoryEntity Factory { get; set; }
    public required string UserId { get; set; }
    public DateTime AddedDate { get; set; } = DateTime.UtcNow;
    public bool IsProcessed { get; set; } = false;
    public DateTime? ProcessedDate { get; set; }
}
