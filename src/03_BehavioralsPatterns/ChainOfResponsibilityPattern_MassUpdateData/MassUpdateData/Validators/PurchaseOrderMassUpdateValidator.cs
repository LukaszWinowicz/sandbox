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
        // === Budowa łańcucha dla pola 'PurchaseOrder' ===
        var poNotEmpty = new NotEmptyValidator(dto => ((MassUpdatePurchaseOrderDto)dto).PurchaseOrder, "Purchase Order");
        var poLength = new StringLengthValidator(dto => ((MassUpdatePurchaseOrderDto)dto).PurchaseOrder, 10, "Purchase Order");
        var poExists = new ExistenceValidator<string>(
             dto => ((MassUpdatePurchaseOrderDto)dto).PurchaseOrder!,
             dataService.OrderExists,
             "Purchase Order"
        );

        // ... tutaj dodalibyśmy walidator kombinacji, ale na razie go pomijamy dla prostoty

        poNotEmpty.SetNext(poLength);
        poLength.SetNext(poExists);

        _purchaseOrderChainHead = poNotEmpty; // Zapisujemy głowę łańcucha

        // === Budowa łańcucha dla pola 'ReceiptDate' ===
        var dateValidator = new FutureDateValidator(
            dto => ((MassUpdatePurchaseOrderDto)dto).ReceiptDate,
            "Receipt Date"
        );
        _receiptDateChainHead = dateValidator; // Ten łańcuch ma tylko jedno ogniwo
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