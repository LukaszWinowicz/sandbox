// === KROK 1: Przygotowanie zależności ===
// Tworzymy jedną instancję serwisu danych, której będą używać wszystkie walidatory
using MassUpdateData.Models;
using MassUpdateData.Services;
using MassUpdateData.Validators;

var dataService = new OrderDataService();

// === KROK 2: Stworzenie i skonfigurowanie ogniw łańcucha ===

// Tworzymy skonfigurowaną instancję walidatora formatu dla zlecenia zakupu (10 znaków)
var formatValidator = new OrderFormatValidator(10, "BŁĄD: Numer zlecenia zakupu musi mieć {0} znaków.");

// Gdybyśmy potrzebowali drugiej, dla zlecenia produkcyjnego, byłoby to:
// var prodFormatValidator = new OrderFormatValidator(8, "BŁĄD: Numer zlecenia produkcyjnego musi mieć {0} znaków.");

// Tworzymy walidatory, wstrzykując im serwis danych
var existenceValidator = new OrderExistenceValidator(dataService);
var combinationValidator = new LineCombinationValidator(dataService);
var dateValidator = new FutureDateValidator(); // Ten nie ma zależności

// === KROK 3: Zbudowanie łańcucha ===
formatValidator.SetNext(existenceValidator);
existenceValidator.SetNext(combinationValidator);
combinationValidator.SetNext(dateValidator);

// === KROK 4: Testowanie ===
var request = new UpdateRequest { Order = "PO2025-001", Line = 2, Sequence = 1, ConfirmationDate = DateTime.Now.AddDays(-10) };

Console.WriteLine($"--- Walidacja zlecenia: {request.Order} ---");
formatValidator.Validate(request); // Uruchamiamy łańcuch

if (request.IsValid)
{
    Console.WriteLine("SUKCES: Żądanie poprawne.");
}
else
{
    Console.WriteLine("BŁĘDY:");
    request.ValidationErrors.ForEach(Console.WriteLine);
}