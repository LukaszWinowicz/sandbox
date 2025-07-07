using KERP.Core.Abstractions.Messaging;
using KERP.Core.Features.MassUpdate.Enums;

namespace KERP.Core.Features.MassUpdate.Commands;

/// <summary>
/// Represents the command to process a request for updating a Purchase Order's receipt date.
/// </summary>
public class PurchaseOrderReceiptDateUpdateCommand : CommandBase, IRequest<List<string>>
{
    public required string PurchaseOrder { get; set; }

    public required int LineNumber { get; set; }

    public required int Sequence { get; set; }

    public required DateTime? ReceiptDate { get; set; }

    public required ReceiptDateUpdateType DateType { get; set; }
}
