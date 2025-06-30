using MassUpdate.Core.Builders;
using MassUpdate.Core.DTOs;
using MassUpdate.Core.Handlers;
using MassUpdate.Core.Interfaces;
using MassUpdate.Core.Interfaces.Repositories;
using MassUpdate.Core.Validators.Components;

namespace MassUpdate.Core.Validators.Orchestrators;

// Ta klasa nie implementuje IValidationHandler, jest samodzielnym orkiestratorem.
public class UpdateReceiptDateValidationStrategy : IValidationStrategy<UpdateReceiptDateDto>
{
    // Wracamy do wielu małych, niezależnych łańcuchów
    private readonly IValidationHandler _purchaseOrderChain;
    private readonly IValidationHandler _lineNumberChain;
    private readonly IValidationHandler _sequenceChain;
    private readonly IValidationHandler _receiptDateChain;
    private readonly IValidationHandler _combinationValidator;

    public UpdateReceiptDateValidationStrategy(IPurchaseOrderValidationRepository validationRepository)
    {
        // Łańcuch dla Purchase Order
        _purchaseOrderChain = new ValidationChainBuilder()
            .WithNotEmptyCheck(dto => ((UpdateReceiptDateDto)dto).PurchaseOrder, "Purchase Order")
            .WithStringLengthCheck(dto => ((UpdateReceiptDateDto)dto).PurchaseOrder, 10, "Purchase Order")
            .WithExistenceCheck<string>(dto => ((UpdateReceiptDateDto)dto).PurchaseOrder!, validationRepository.OrderExistsAsync, "Purchase Order")
            .Build();

        // Łańcuch dla Line Number
        _lineNumberChain = new ValidationChainBuilder()
            .WithMinValueCheck<int>(dto => ((UpdateReceiptDateDto)dto).LineNumber, 1, "Line Number")
            .Build();

        // Łańcuch dla Sequence
        _sequenceChain = new ValidationChainBuilder()
            .WithMinValueCheck<int>(dto => ((UpdateReceiptDateDto)dto).Sequence, 1, "Sequence")
            .Build();

        // Łańcuch dla Receipt Date
        _receiptDateChain = new ValidationChainBuilder()
            .WithNotNullCheck<DateTime?>(dto => ((UpdateReceiptDateDto)dto).ReceiptDate, "Receipt Date")
            .WithFutureDateCheck(dto => ((UpdateReceiptDateDto)dto).ReceiptDate, "Receipt Date")
            .Build();

        // Specjalny walidator dla kombinacji
        _combinationValidator = new CombinationValidator<UpdateReceiptDateDto, IPurchaseOrderValidationRepository>(
            validationRepository,
            (dto, repo) => repo.CombinationExistsAsync(dto.PurchaseOrder, dto.LineNumber, dto.Sequence).Result, // Używamy .Result, bo Func jest synchroniczny
            "Combination of PO, Line, and Sequence does not exist."
        );
    }

    public async Task<List<string>> ValidateAsync(UpdateReceiptDateDto dto)
    {
        var request = new ValidationRequest(dto);

        // Uruchamiamy wszystkie łańcuchy asynchronicznie
        var purchaseOrderTask = _purchaseOrderChain.ValidateAsync(request);
        var lineNumberTask = _lineNumberChain.ValidateAsync(request);
        var sequenceTask = _sequenceChain.ValidateAsync(request);
        var receiptDateTask = _receiptDateChain.ValidateAsync(request);

        await Task.WhenAll(purchaseOrderTask, lineNumberTask, sequenceTask, receiptDateTask);

        // Walidację kombinacji uruchamiamy na końcu, jeśli nie ma jeszcze błędów
        if (request.IsValid)
        {
            await _combinationValidator.ValidateAsync(request);
        }

        return request.ValidationErrors;
    }
}