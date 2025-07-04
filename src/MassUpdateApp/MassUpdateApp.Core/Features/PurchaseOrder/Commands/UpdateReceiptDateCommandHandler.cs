using MassUpdateApp.Core.Abstractions.Messaging;
using MassUpdateApp.Core.DTOs;

namespace MassUpdateApp.Core.Features.PurchaseOrder.Commands;

/// <summary>
/// Handles the <see cref="UpdateReceiptDateCommand"/>
/// </summary>
public class UpdateReceiptDateCommandHandler
    :IRequestHandler<UpdateReceiptDateCommand, List<string>>
{
    private readonly IValidationStrategy<UpdateReceiptDateDto> _validationStrategy;
    private readonly ICommandRepository _commandRepository;

    public UpdateReceiptDateCommandHandler()
    {
        
    }
}
