using KERP.Application.Interfaces;
using KERP.Application.MassUpdate.PurchaseOrder.Commands;
using KERP.Application.Shared.Validation;
using KERP.Domain.Interfaces.MassUpdate.PurchaseOrder;

namespace KERP.Application.MassUpdate.PurchaseOrder.Validation;

public class ReceiptDateUpdateValidator : IReceiptDateUpdateValidator
{
    private readonly IPurchaseOrderRepository _purchaseOrderRepository;
    private readonly ICurrentUserService _currentUserService;

    public ReceiptDateUpdateValidator(
        IPurchaseOrderRepository purchaseOrderRepository,
        ICurrentUserService currentUserService)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
        _currentUserService = currentUserService;
    }

    public async Task<List<string>> ValidateAsync(PurchaseOrderUpdateDto dto)
    {
        var request = new ValidationRequest<PurchaseOrderUpdateDto>(dto);

        // Tworzymy i uruchamiamy potok w jednej metodzie
        var pipeline = new ValidationPipelineBuilder<PurchaseOrderUpdateDto>()
            .WithNotEmpty(d => d.PurchaseOrder)
            .WithStringLength(d => d.PurchaseOrder, 10)
            .WithMinValue(d => d.LineNumber, 1)
            .WithFutureDate(d => d.ReceiptDate)
            // Używamy naszych metod rozszerzających
            .WithOrderExistenceCheck(_purchaseOrderRepository, _currentUserService)
            .WithCombinationCheck(_purchaseOrderRepository, _currentUserService)
            .Build();

        await pipeline.ValidateAsync(request);

        return request.Errors;
    }
}