using KERP.Application.Abstractions.Messaging;
using KERP.Domain.MassUpdate.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace KERP.Application.Features.MassUpdate.Commands;

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

    [NotMapped] // Ważne, aby EF Core nie próbował tego mapować, jeśli kiedyś będziemy go używać jako encji
    public string DateTypeAsString
    {
        get => DateType.ToString();
        set => DateType = Enum.Parse<ReceiptDateUpdateType>(value);
    }
}
