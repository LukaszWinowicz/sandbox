using KERP.Core.Features.MassUpdate.Enums;

namespace KERP.Core.Features.MassUpdate.DTOs;

/// <summary>
/// Data Transfer Object (DTO) for 'Update Receipt Date' operation on a Purchase Order.
/// </summary>
public class UpdateReceiptDateDto : MassUpdateDto
{
    /// <summary>
    /// The Purchase Order number.
    /// </summary>
    public required string PurchaseOrder { get; set; }

    /// <summary>
    /// The line number on the Purchase Order.
    /// </summary>
    public required int LineNumber { get; set; }

    /// <summary>
    /// The sequence number on the Purchase Order line.
    /// </summary>
    public required int Sequence { get; set; }

    /// <summary>
    /// The new receipt date. Can be null if not yet selected by the user in the UI.
    /// </summary>
    public required DateTime? ReceiptDate { get; set; }

    /// <summary>
    /// The type of the date update (e.g., Confirmed, Changed).
    /// </summary>
    public required ReceiptDateUpdateType DateType { get; set; }
}
