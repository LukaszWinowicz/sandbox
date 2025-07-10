using KERP.Application.Interfaces;
using KERP.Application.MassUpdate.PurchaseOrder.Commands;
using KERP.Application.Shared.Validation;
using KERP.Application.Shared.Validation.Validators;
using KERP.Domain.Interfaces.MassUpdate.PurchaseOrder;

namespace KERP.Application.MassUpdate.PurchaseOrder.Validators;

public class ValidationPipelineFactory : IValidationPipelineFactory
{
    private readonly IPurchaseOrderRepository _purchaseOrderRepository;
    private readonly ICurrentUserService _currentUserService;

    // Fabryka potrzebuje dostępu do zależności, których będą potrzebować jej walidatory.
    public ValidationPipelineFactory(
        IPurchaseOrderRepository purchaseOrderRepository,
        ICurrentUserService currentUserService)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
        _currentUserService = currentUserService;
    }

    public IValidationHandler<PurchaseOrderUpdateDto> CreateForReceiptDateUpdate()
    {
        var builder = new ValidationPipelineBuilder<PurchaseOrderUpdateDto>();

        // Składamy nasz potok, zaczynając od najprostszych i najszybszych walidacji,
        // a kończąc na tych najdroższych (odpytujących bazę danych).
        builder
            // Generyczne, reużywalne walidatory
            .Add(new NotEmptyValidator<PurchaseOrderUpdateDto>(dto => dto.PurchaseOrder))
            .Add(new StringLengthValidator<PurchaseOrderUpdateDto>(dto => dto.PurchaseOrder, 10))
            .Add(new MinValueValidator<PurchaseOrderUpdateDto, int>(dto => dto.LineNumber, 1))
            .Add(new FutureDateValidator<PurchaseOrderUpdateDto>(dto => dto.ReceiptDate, "Receipt Date"))

            // Specyficzne walidatory dla tej funkcji, wymagające dostępu do bazy
            .Add(new OrderExistenceValidator(_purchaseOrderRepository, _currentUserService))
            .Add(new CombinationValidator(_purchaseOrderRepository, _currentUserService));

        return builder.Build();
    }
}
