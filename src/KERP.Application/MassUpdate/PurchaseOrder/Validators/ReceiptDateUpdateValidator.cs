using KERP.Application.Interfaces;
using KERP.Application.MassUpdate.PurchaseOrder.Commands;
using KERP.Application.Shared.Validation;
using KERP.Domain.Interfaces.MassUpdate.PurchaseOrder;

namespace KERP.Application.MassUpdate.PurchaseOrder.Validators;

public class ReceiptDateUpdateValidator : IReceiptDateUpdateValidator
{
    private readonly IValidationHandler<PurchaseOrderUpdateDto> _purchaseOrderChain;
    private readonly IValidationHandler<PurchaseOrderUpdateDto> _lineNumberChain;
    private readonly IValidationHandler<PurchaseOrderUpdateDto> _receiptDateChain;
    private readonly IValidationHandler<PurchaseOrderUpdateDto> _combinationChain;

    public ReceiptDateUpdateValidator(
        IPurchaseOrderRepository purchaseOrderRepository,
        ICurrentUserService currentUserService)
    {
        // --- POPRAWIONA SEKCJA ---
        // Używamy teraz naszego nowego, płynnego buildera

        // --- Budowa łańcucha dla pola 'PurchaseOrder' ---
        _purchaseOrderChain = new ValidationPipelineBuilder<PurchaseOrderUpdateDto>(purchaseOrderRepository, currentUserService)
            .WithNotEmpty(dto => dto.PurchaseOrder)
            .WithStringLength(dto => dto.PurchaseOrder, 10)
            .WithOrderExistenceCheck()
            .Build();

        // --- Budowa łańcucha dla pola 'LineNumber' ---
        _lineNumberChain = new ValidationPipelineBuilder<PurchaseOrderUpdateDto>()
            .WithMinValue(dto => dto.LineNumber, 1)
            .Build();

        // --- Budowa łańcucha dla pola 'ReceiptDate' ---
        _receiptDateChain = new ValidationPipelineBuilder<PurchaseOrderUpdateDto>()
            .WithFutureDate(dto => dto.ReceiptDate)
            .Build();

        // --- Osobny łańcuch dla walidacji kombinacji pól ---
        _combinationChain = new ValidationPipelineBuilder<PurchaseOrderUpdateDto>(purchaseOrderRepository, currentUserService)
            .WithCombinationCheck()
            .Build();
    }

    public async Task<List<string>> ValidateAsync(PurchaseOrderUpdateDto dto)
    {
        var request = new ValidationRequest<PurchaseOrderUpdateDto>(dto);

        var validationTasks = new List<Task>
        {
            _purchaseOrderChain.ValidateAsync(request),
            _lineNumberChain.ValidateAsync(request),
            _receiptDateChain.ValidateAsync(request)
        };

        await Task.WhenAll(validationTasks);

        if (request.IsValid)
        {
            await _combinationChain.ValidateAsync(request);
        }

        return request.Errors;
    }
}