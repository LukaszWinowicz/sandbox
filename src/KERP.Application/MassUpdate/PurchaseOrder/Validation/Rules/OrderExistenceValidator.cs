using KERP.Application.Interfaces;
using KERP.Application.MassUpdate.PurchaseOrder.Commands;
using KERP.Application.Shared.Validation;
using KERP.Domain.Interfaces.MassUpdate.PurchaseOrder;

namespace KERP.Application.MassUpdate.PurchaseOrder.Validation;

public class OrderExistenceValidator : AbstractValidationHandler<PurchaseOrderUpdateDto>
{
    private readonly IPurchaseOrderRepository _purchaseOrderRepository;
    private readonly ICurrentUserService _currentUserService;

    // Zależności są jawnie wstrzykiwane przez konstruktor
    public OrderExistenceValidator(
        IPurchaseOrderRepository purchaseOrderRepository,
        ICurrentUserService currentUserService)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
        _currentUserService = currentUserService;
    }

    protected override async Task HandleValidation(ValidationRequest<PurchaseOrderUpdateDto> request)
    {
        var factoryId = _currentUserService.SelectedFactoryId ?? 0;
        var purchaseOrder = request.DtoToValidate.PurchaseOrder;

        // Sprawdzamy tylko, jeśli pole nie jest puste (za to odpowiada NotEmptyValidator)
        if (factoryId > 0 && !string.IsNullOrWhiteSpace(purchaseOrder))
        {
            bool exists = await _purchaseOrderRepository.OrderExistsAsync(purchaseOrder, factoryId);
            if (!exists)
            {
                request.Errors.Add($"Purchase Order '{purchaseOrder}' does not exist in factory {factoryId}.");
            }
        }
    }
}