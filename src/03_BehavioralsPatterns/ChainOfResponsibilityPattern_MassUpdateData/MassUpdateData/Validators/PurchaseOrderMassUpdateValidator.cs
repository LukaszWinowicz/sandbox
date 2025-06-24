using MassUpdateData.Builders;
using MassUpdateData.Handlers;
using MassUpdateData.Models;
using MassUpdateData.Services;

namespace MassUpdateData.Validators;

// Ta klasa nie implementuje IValidationHandler, jest samodzielnym orkiestratorem.
public class PurchaseOrderMassUpdateValidator
{
    // Przechowujemy tylko pierwsze ogniwa każdego z łańcuchów
    private readonly IValidationHandler _purchaseOrderChainHead;
    private readonly IValidationHandler _receiptDateChainHead;

    public PurchaseOrderMassUpdateValidator(IOrderDataService dataService)
    {
        _purchaseOrderChainHead = new ValidationChainBuilder()
             .WithNotEmptyCheck(dto => ((MassUpdatePurchaseOrderDto)dto).PurchaseOrder, "Purchase Order")
             .WithStringLengthCheck(dto => ((MassUpdatePurchaseOrderDto)dto).PurchaseOrder, 10, "Purchase Order")
             .WithExistenceCheck<string>(dto => ((MassUpdatePurchaseOrderDto)dto).PurchaseOrder!, dataService.OrderExists, "Purchase Order")
             .Build();

        _receiptDateChainHead = new ValidationChainBuilder()
            .WithFutureDateCheck(dto => ((MassUpdatePurchaseOrderDto)dto).ReceiptDate, "Receipt Date")
            .Build();
    }

    // Główna metoda walidująca cały obiekt DTO
    public List<string> Validate(MassUpdatePurchaseOrderDto dto)
    {
        var request = new ValidationRequest(dto);

        // Uruchamiamy po kolei każdy z naszych głównych łańcuchów
        _purchaseOrderChainHead.Validate(request);
        _receiptDateChainHead.Validate(request);

        return request.ValidationErrors;
    }
}