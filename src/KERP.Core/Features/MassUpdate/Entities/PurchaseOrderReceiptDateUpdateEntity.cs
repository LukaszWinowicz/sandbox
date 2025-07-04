using KERP.Core.Features.MassUpdate.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KERP.Core.Features.MassUpdate.Entities;

/// <summary>
/// Represents a single, persisted record of a request to update a Purchase Order's receipt date.
/// This entity distinguishes between different types of date updates (e.g., Confirmed vs. Changed)
/// to support downstream processing by the ERP System.
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

    [Required]
    DateTime AddedDate { get; set; } = DateTime.UtcNow;

    public bool IsProcessed { get; set; } = false;

    public DateTime? ProcessedDate { get; set; }

}
