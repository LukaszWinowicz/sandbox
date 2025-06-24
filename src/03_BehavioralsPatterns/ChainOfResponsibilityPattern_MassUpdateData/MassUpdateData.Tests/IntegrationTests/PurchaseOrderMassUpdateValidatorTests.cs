using MassUpdate.Core.DTOs;
using MassUpdate.Core.Interfaces;
using MassUpdate.Core.Validators.Components;
using MassUpdate.Infrastructure;
using Moq;

namespace MassUpdateData.Tests.IntegrationTests;

public class PurchaseOrderMassUpdateValidatorTests
{
    [Fact]
    public void Validate_ShouldReturnNoErrors_WhenDtoIsValid()
    {
        // ARRANGE
        // 1. Tworzymy mock serwisu danych
        var mockDataService = new Mock<IOrderDataService>();

        // 2. Tworzymy w pełni poprawny obiekt DTO do testu.        
        var validDto = new PurchaseOrderDtoBuilder().Build();

        // 3. "Uczymy" go, jak ma odpowiadać. Dla "Happy Path", wszystkie odpowiedzi są pozytywne.
        // Mówimy: "Dla dowolnego stringa (It.IsAny<string>()), metoda OrderExists zawsze zwróci `true`".
        mockDataService.Setup(service => service.OrderExists(It.IsAny<string>())).Returns(true);
        mockDataService.Setup(service => service.LineCombinationExists(validDto.PurchaseOrder, validDto.LineNumber, validDto.Sequence)).Returns(true);

        // 4. Tworzymy instancję naszego głównego walidatora, wstrzykując mu zamockowany serwis.
        var mainValidator = new PurchaseOrderMassUpdateValidator(mockDataService.Object);
         
        // ACT
        // Wywołujemy główną metodę walidującą.
        List<string> validationErrors = mainValidator.Validate(validDto);

        // ASSERT
        // Sprawdzamy, czy rezultat jest zgodny z naszymi oczekiwaniami.
        // Dla "Happy Path" lista błędów powinna być pusta.
        Assert.Empty(validationErrors);
    }

    [Fact]
    public void Validate_ShouldReturnSingleError_WhenPoNumberIsTooShort()
    {
        // ARRANGE
        // 1. Mock serwisu danych. W tym konkretnym teście jego zachowanie nie ma znaczenia,
        // ponieważ walidacja nie powinna do niego dotrzeć (zatrzyma się na walidacji długości),
        // ale dobrą praktyką jest zawsze go przygotować.
        var mockDataService = new Mock<OrderDataService>();

        // 2. Tworzymy instancję naszego głównego walidatora.
        var mainValidator = new PurchaseOrderMassUpdateValidator(mockDataService.Object);

        // 3. Czytelnie tworzymy DTO, które jest domyślnie poprawne,
        // a następnie wprowadzamy JEDNĄ, konkretną zmianę na potrzeby testu.
        var invalidDto = new PurchaseOrderDtoBuilder()
            .WithPurchaseOrder("PO0123") // Zmieniamy tylko to co testujemy
            .Build();

        // ACT
        // Wywołujemy główną metodę walidującą.
        List<string> validationErrors = mainValidator.Validate(invalidDto);

        // ASSERT
        // 1. Sprawdzamy, czy lista błędów zawiera DOKŁADNIE jeden element.
        Assert.Single(validationErrors);

        // 2. Sprawdzamy, czy treść tego błędu jest prawidłowa.
        Assert.Equal("Purchase Order must be exactly 10 characters long.", validationErrors[0]);
    }

    [Fact]
    public void Validate_ShouldAggregateErrors_WhenMultipleFieldsAreInvalid()
    {
        // ARRANGE
        // 1. Mock serwisu danych. Jego konfiguracja nie jest kluczowa, bo błędy
        //    powinny zostać wykryte przed jakimkolwiek zapytaniem do serwisu.
        var mockDataService = new Mock<IOrderDataService>();

        // 2. Tworzymy instancję naszego głównego walidatora.
        var mainValidator = new PurchaseOrderMassUpdateValidator(mockDataService.Object);

        // 3. Tworzymy DTO z KILKOMA błędami:
        //    - `PurchaseOrder` jest za krótki.
        //    - `ReceiptDate` jest datą z przeszłości.
        var invalidDto = new MassUpdatePurchaseOrderDto
        {
            PurchaseOrder = "PO123", // Błąd 1: Niepoprawna długość
            LineNumber = 10,
            Sequence = 1,
            ReceiptDate = new DateTime(2020, 1, 1) // Błąd 2: Data z przeszłości
        };

        // ACT
        // Wywołujemy główną metodę walidującą.
        List<string> validationErrors = mainValidator.Validate(invalidDto);

        // ASSERT
        // 1. Sprawdzamy, czy lista błędów zawiera DOKŁADNIE dwa elementy.
        Assert.Equal(2, validationErrors.Count);

        // 2. Sprawdzamy, czy na liście znajdują się OBA oczekiwane komunikaty.
        //    Używamy Assert.Contains, aby nie martwić się o kolejność, w jakiej błędy zostały dodane.
        Assert.Contains("Purchase Order must be exactly 10 characters long.", validationErrors);
        Assert.Contains("Receipt Date must be today or a future date.", validationErrors);
    }

    [Fact]
    public void Validate_ShouldReturnCombinationError_WhenFieldsAreValidButCombinationDoesNotExist()
    {
        // ARRANGE
        // 1. Przygotowujemy dane wejściowe. Wszystkie pola są w sobie poprawne
        string validPoNumber = "PO12345678";
        int validLine = 20;
        int validSeq = 5;

        // 2. Tworzymy mock serwisu danych.
        var mockDataService = new Mock<IOrderDataService>();

        // 3.Konfigurujemy zachowanie mocka:
        //    - Sprawdzenie istnienia samego numeru PO zwraca `true` (przechodzi walidację w małym łańcuchu).
        mockDataService.Setup(service => service.OrderExists(validPoNumber)).Returns(true);
        //    - Sprawdzenie KOMBINACJI zwraca `false` (to jest nasz scenariusz błędu).
        mockDataService.Setup(service => service.LineCombinationExists(validPoNumber, validLine, validSeq)).Returns(false);

        // 4. Tworzymy instancję naszego głównego walidatora, wstrzykując mu zamockowany serwis.
        var mainValidator = new PurchaseOrderMassUpdateValidator(mockDataService.Object);

        // 5. Tworzymy DTO z danymi, które indywidaulnie są poprawne.
        var dtoWithInvalidCombination = new MassUpdatePurchaseOrderDto
        {
            PurchaseOrder = validPoNumber,
            LineNumber = validLine,
            Sequence = validSeq,
            ReceiptDate = DateTime.Now.AddDays(1)
        };

        // ACT
        // Wywołujemy główną metodę walidującą.
        List<string> validationErrors = mainValidator.Validate(dtoWithInvalidCombination);

        // ASSERT
        // 1. Oczekujemy dokładnie jednego błędu.
        Assert.Single(validationErrors);

        // 2. Sprawdzamy, czy treść błędu pochodzi z naszego 'CombinationValidator'.
        Assert.Contains("Combination of PO", validationErrors[0]);
        Assert.Contains("does not exist", validationErrors[0]);


    }

}