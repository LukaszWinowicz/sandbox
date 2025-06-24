// 1. Inicjalizacja serwisu
using MassUpdateData.Models;
using MassUpdateData.Services;
using MassUpdateData.Validators;
using MassUpdateData.Validators.Components;

var dataService = new OrderDataService();

// 2. Tworzenie instancji "klocków" walidacyjnych
var poNotEmpty = new NotEmptyValidator(dto => ((MassUpdatePurchaseOrderDto)dto).PurchaseOrder, "Purchase Order");
var poLength = new StringLengthValidator(dto => ((MassUpdatePurchaseOrderDto)dto).PurchaseOrder, 10, "Purchase Order");

// POPRAWKA: Tworzymy instancję generycznego, synchronicznego walidatora
var poExists = new ExistenceValidator<string>(
    dto => ((MassUpdatePurchaseOrderDto)dto).PurchaseOrder!, // Pobierz wartość pola
    dataService.OrderExists,                                 // Przekaż metodę z serwisu jako funkcję sprawdzającą
    "Purchase Order"                                         // Nazwa pola do komunikatu błędu
);

// POPRAWKA: Tworzymy instancję generycznego walidatora,
// "ucząc" go, skąd ma brać datę (`ReceiptDate`) i jak nazywa się to pole.
var dateValidator = new FutureDateValidator(
    dto => ((MassUpdatePurchaseOrderDto)dto).ReceiptDate,
    "Receipt Date"
);

// 3. Ręczne łączenie ogniw w łańcuch
poNotEmpty.SetNext(poLength);
poLength.SetNext(poExists);
poExists.SetNext(dateValidator);

// 4. Uruchomienie walidacji (już bez 'await')
var firstHandler = poNotEmpty;
var request = new ValidationRequest(new MassUpdatePurchaseOrderDto
{
    PurchaseOrder = "ZLY_NUMER",
    ReceiptDate = new DateTime(2020, 1, 1)
});

firstHandler.Validate(request);

// 5. Wyświetlenie wyników
if (request.IsValid)
{
    Console.WriteLine("Validation successful!");
}
else
{
    Console.WriteLine("Validation failed with errors:");
    foreach (var error in request.ValidationErrors)
    {
        Console.WriteLine($"- {error}");
    }
}