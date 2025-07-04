using MassUpdateApp.Core.Abstractions.Messaging;
using MassUpdateApp.Core.DTOs;

namespace MassUpdateApp.Core.Features.PurchaseOrder.Commands;

/// <summary>
/// Represents the command to process a request for updating a purchase order's receipt date.
/// </summary>
/// <param name="Dto">The Data Transfer Object containing the details for updating the receipt date.</param>
public record UpdateReceiptDateCommand(UpdateReceiptDateDto Dto) : IRequest<List<string>>;

