using KERP.Application.Builders;
using KERP.Application.Features.MassUpdate.Commands;
using KERP.Application.Interfaces.ValidationStrategies;
using KERP.Application.Validation.Components;
using KERP.Application.Validation.Context;
using KERP.Application.Validation.Handlers;
using KERP.Domain.Interfaces.Repositories;

namespace KERP.Application.ValidationStrategies;

/// <summary>
/// Implements the validation strategy for the PurchaseOrderReceiptDateUpdateCommand.
/// It builds and executes multiple validation chains for each relevant field.
/// </summary>
public class PurchaseOrderReceiptDateUpdateStrategy : IValidationStrategy<PurchaseOrderReceiptDateUpdateCommand>
{
    private readonly IValidationHandler _purchaseOrderChain;
    private readonly IValidationHandler _lineNumberChain;
    private readonly IValidationHandler _sequenceChain;
    private readonly IValidationHandler _receiptDateChain;
    private readonly IValidationHandler _combinationValidator;

    public PurchaseOrderReceiptDateUpdateStrategy(IPurchaseOrderValidationRepository validationRepository)
    {
        // --- Budowa Łańcuchów Walidacji ---

        _purchaseOrderChain = new ValidationChainBuilder()
            .WithNotEmptyCheck(cmd => ((PurchaseOrderReceiptDateUpdateCommand)cmd).PurchaseOrder, "Purchase Order")
            .WithStringLengthCheck(cmd => ((PurchaseOrderReceiptDateUpdateCommand)cmd).PurchaseOrder, 10, "Purchase Order")
            .WithExistenceCheck(cmd => ((PurchaseOrderReceiptDateUpdateCommand)cmd).PurchaseOrder, validationRepository.OrderExistsAsync, "Purchase Order")
            .Build();

        _lineNumberChain = new ValidationChainBuilder()
            .WithMinValueCheck(cmd => ((PurchaseOrderReceiptDateUpdateCommand)cmd).LineNumber, 10, "Line Number")
            .Build();

        _sequenceChain = new ValidationChainBuilder()
            .WithMinValueCheck(cmd => ((PurchaseOrderReceiptDateUpdateCommand)cmd).Sequence, 1, "Sequence")
            .Build();

        _receiptDateChain = new ValidationChainBuilder()
            .WithNotNullCheck(cmd => ((PurchaseOrderReceiptDateUpdateCommand)cmd).ReceiptDate, "Receipt Date")
            .WithFutureDateCheck(cmd => ((PurchaseOrderReceiptDateUpdateCommand)cmd).ReceiptDate, "Receipt Date")
            .Build();

        _combinationValidator = new CombinationValidator<PurchaseOrderReceiptDateUpdateCommand, IPurchaseOrderValidationRepository>(
            validationRepository,
            async (cmd, repo) => await repo.CombinationExistsAsync(cmd.PurchaseOrder, cmd.LineNumber, cmd.Sequence),
            "The combination of PO, Line, and Sequence does not exist."
        );
    }

    /// <inheritdoc />
    public async Task<List<string>> ValidateAsync(PurchaseOrderReceiptDateUpdateCommand command)
    {
        var request = new ValidationRequest(command);

        // Uruchamiamy wszystkie łańcuchy dla poszczególnych pól, potencjalnie równolegle
        var validationTasks = new List<Task>
            {
                _purchaseOrderChain.ValidateAsync(request),
                _lineNumberChain.ValidateAsync(request),
                _sequenceChain.ValidateAsync(request),
                _receiptDateChain.ValidateAsync(request)
            };

        await Task.WhenAll(validationTasks);

        // Walidację kombinacji uruchamiamy na końcu, tylko jeśli pojedyncze pola są poprawne
        // (na razie uproszczenie - sprawdzamy, czy nie ma błędów do tej pory)
        if (request.IsValid)
        {
            await _combinationValidator.ValidateAsync(request);
        }

        return request.Errors;
    }
}
