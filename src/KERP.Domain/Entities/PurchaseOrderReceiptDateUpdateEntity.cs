using KERP.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KERP.Domain.Entities;

/// <summary>
/// Represents a single, persisted record of a request to update a Purchase Order's receipt date.
/// </summary>
public class PurchaseOrderReceiptDateUpdateEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(10)]
    public required string PurchaseOrder { get; set; }

    [Required]
    public required int LineNumber { get; set; }

    [Required]
    public required int Sequence { get; set; }

    [Required]
    [Column(TypeName = "date")]
    public required DateTime ReceiptDate { get; set; }

    [Required]
    public required ReceiptDateUpdateType DateType { get; set; }

    [Required]
    public required string UserId { get; set; }

    /// <summary>
    /// The timestamp when the record was added to the database.
    /// </summary>
    public DateTime AddedDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// A flag indicating if this request has been processed and sent to the ERP system.
    /// Defaults to false.
    /// </summary>
    public bool IsProcessed { get; set; } = false;

    /// <summary>
    /// The timestamp when the request was processed. Null if not yet processed.
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? ProcessedDate { get; set; }
}
