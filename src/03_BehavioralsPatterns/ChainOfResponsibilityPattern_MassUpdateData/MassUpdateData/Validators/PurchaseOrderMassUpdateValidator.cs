using MassUpdateData.Builders;
using MassUpdateData.Handlers;
using MassUpdateData.Models;
using MassUpdateData.Services;
using MassUpdateData.Validators.Components;

namespace MassUpdateData.Validators;

// Ta klasa nie implementuje IValidationHandler, jest samodzielnym orkiestratorem.
public class PurchaseOrderMassUpdateValidator
{
    // Przechowujemy tylko pierwsze ogniwa każdego z łańcuchów
    private readonly IValidationHandler _purchaseOrderChain;
    private readonly IValidationHandler _lineNumberChain;
    private readonly IValidationHandler _sequenceChain;
    private readonly IValidationHandler _receiptDateChain;

    // Specjalny walidator dla kombinacji, uruchamiany osobno
    private readonly IValidationHandler _combinationValidator;

    public PurchaseOrderMassUpdateValidator(IOrderDataService dataService)
    {
        _purchaseOrderChain = new ValidationChainBuilder()
           .WithNotEmptyCheck(dto => ((MassUpdatePurchaseOrderDto)dto).PurchaseOrder, "Purchase Order")
           .WithStringLengthCheck(dto => ((MassUpdatePurchaseOrderDto)dto).PurchaseOrder, 10, "Purchase Order")
           .WithExistenceCheck<string>(dto => ((MassUpdatePurchaseOrderDto)dto).PurchaseOrder!, dataService.OrderExists, "Purchase Order")
           .Build();

        _lineNumberChain = new ValidationChainBuilder()
            .WithMinValueCheck<int>(dto => ((MassUpdatePurchaseOrderDto)dto).LineNumber, 10, "Line Number")
            .Build();

        _sequenceChain = new ValidationChainBuilder()
            .WithMinValueCheck<int>(dto => ((MassUpdatePurchaseOrderDto)dto).Sequence, 1, "Sequence")
            .Build();

        _receiptDateChain = new ValidationChainBuilder()
            .WithFutureDateCheck(dto => ((MassUpdatePurchaseOrderDto)dto).ReceiptDate, "Receipt Date")
            .Build();

        // W konstruktorze PurchaseOrderMassUpdateValidator
        _combinationValidator = new CombinationValidator<MassUpdatePurchaseOrderDto, IOrderDataService>(
            // 1. Argument: Serwis danych, którego ma użyć
            dataService,

            // 2. Argument: "Przepis" na walidację w formie wyrażenia lambda
            (dto, service) => service.LineCombinationExists(dto.PurchaseOrder!, dto.LineNumber, dto.Sequence),

            // 3. Argument: Komunikat błędu
            "Combination of PO, Line, and Sequence does not exist."
        );
    }

    // Główna metoda walidująca cały obiekt DTO
    public List<string> Validate(MassUpdatePurchaseOrderDto dto)
    {
        var request = new ValidationRequest(dto);

        _purchaseOrderChain.Validate(request);
        _lineNumberChain.Validate(request);
        _sequenceChain.Validate(request);
        _receiptDateChain.Validate(request);

        // Uruchom walidację kombinacji tylko jeśli podstawowe pola są poprawne
        if (request.IsValid)
        {
            _combinationValidator.Validate(request);
        }

        return request.ValidationErrors;
    }
}