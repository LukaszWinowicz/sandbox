using KERP.Application.Interfaces;
using KERP.Application.MassUpdate.PurchaseOrder.Commands;
using KERP.Application.Shared.Validation;
using KERP.Domain.Interfaces.MassUpdate.PurchaseOrder;

namespace KERP.Application.MassUpdate.PurchaseOrder.Validators;

public class CombinationValidator : AbstractValidationHandler<PurchaseOrderUpdateDto>
{
    private readonly IPurchaseOrderRepository _purchaseOrderRepository;
    private readonly ICurrentUserService _currentUserService;

    public CombinationValidator(
        IPurchaseOrderRepository purchaseOrderRepository,
        ICurrentUserService currentUserService)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
        _currentUserService = currentUserService;
    }

    protected override async Task HandleValidation(ValidationRequest<PurchaseOrderUpdateDto> request)
    {
        var factoryId = _currentUserService.SelectedFactoryId ?? 0;
        var dto = request.DtoToValidate;

        // Sprawdzamy tylko jeśli mamy wszystkie potrzebne dane
        if (factoryId > 0 && !string.IsNullOrWhiteSpace(dto.PurchaseOrder) && dto.LineNumber > 0)
        {
            bool exists = await _purchaseOrderRepository.CombinationExistsAsync(dto.PurchaseOrder, dto.LineNumber, dto.Sequence, factoryId);
            if (!exists)
            {
                request.Errors.Add($"Combination PO '{dto.PurchaseOrder}', Line '{dto.LineNumber}', Sequence '{dto.Sequence}' does not exist.");
            }
        }
    }
}