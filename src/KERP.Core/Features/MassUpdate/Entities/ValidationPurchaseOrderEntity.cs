using System.ComponentModel.DataAnnotations;

namespace KERP.Core.Features.MassUpdate.Entities;

/// <summary>
/// Represents a read-only view of a Purchase Order's core identifiers,
/// used for validation purposes. This data is typically synchronized
/// from an external system of record (e.g., BigQuery).
/// </summary>
public class ValidationPurchaseOrderEntity
{
    // Uwaga: Ta encja nie ma pojedynczego klucza Id.
    // Jej unikalność jest definiowana przez kombinację trzech poniższych pól.
    // Skonfigurujemy ten "klucz złożony" później w AppDbContext.

    /// <summary>
    /// The Purchase Order number.
    /// </summary>
    [Required]
    [MaxLength(10)]
    public required string PurchaseOrder { get; set; }

    /// <summary>
    /// The line number on the Purchase Order.
    /// </summary>
    [Required]
    public required int LineNumber { get; set; }

    /// <summary>
    /// The sequence number on the Purchase Order line.
    /// </summary>
    [Required]
    public required int Sequence { get; set; }
}
