using MassUpdateData.Models;
using MassUpdateData.Services;
using MassUpdateData.Validators;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace MassUpdateData.Tests.IntegrationTests;

public class PurchaseOrderMassUpdateValidatorTests
{
    [Fact]
    public void Validate_ShouldReturnNoErrors_WhenDtoIsValid()
    {
        // ARRANGE
        // 1. Tworzymy mock serwisu danych
        var mockDataService = new Mock<IOrderDataService>();

        // 2. "Uczymy" go, jak ma odpowiadać. Dla "Happy Path", wszystkie odpowiedzi są pozytywne.
        // Mówimy: "Dla dowolnego stringa (It.IsAny<string>()), metoda OrderExists zawsze zwróci `true`".
        mockDataService.Setup(service => service.OrderExists(It.IsAny<string>())).Returns(true);

        // 3. Tworzymy instancję naszego głównego walidatora, wstrzykując mu zamockowany serwis.
        var mainValidator = new PurchaseOrderMassUpdateValidator(mockDataService.Object);

        // 4. Tworzymy w pełni poprawny obiekt DTO do testu.
        var validDto = new MassUpdatePurchaseOrderDto
        {
            PurchaseOrder = "PO12345678",
            LineNumber = 10,
            Sequence = 1,
            ReceiptDate = DateTime.Now.AddDays(1)
        };

        // ACT
        // Wywołujemy główną metodę walidującą.
        List<string> validationErrors = mainValidator.Validate(validDto);

        // ASSERT
        // Sprawdzamy, czy rezultat jest zgodny z naszymi oczekiwaniami.
        // Dla "Happy Path" lista błędów powinna być pusta.
        Assert.Empty(validationErrors);
    }

    [Fact]
    public void Validate_ShouldReturnSigleError_WhenPoNumberIsTooShort()
    {
        // ARRANGE
        // 1. Mock serwisu danych. W tym konkretnym teście jego zachowanie nie ma znaczenia,
        // ponieważ walidacja nie powinna do niego dotrzeć (zatrzyma się na walidacji długości),
        // ale dobrą praktyką jest zawsze go przygotować.
        var mockDataService = new Mock<IOrderDataService>();

        // 2. Tworzymy instancję naszego głównego walidatora.
        var mainValidator = new PurchaseOrderMassUpdateValidator(mockDataService.Object);

        // 3. Tworzymy DTO z JEDNYM błędem: numer PO jest za krótki (ma 5 znaków zamiast 10).
        var invalidDto = new MassUpdatePurchaseOrderDto
        {
            PurchaseOrder = "PO123", // Niepoprawna długość
            LineNumber = 10,
            Sequence = 1,
            ReceiptDate = DateTime.Now.AddDays(1)
        };

        // ACT
        // Wywołujemy główną metodę walidującą.
        List<string> validationErrors = mainValidator.Validate(invalidDto);

        // ASSERT
        // 1. Sprawdzamy, czy lista błędów zawiera DOKŁADNIE jeden element.
        Assert.Single(validationErrors);

        // 2. Sprawdzamy, czy treść tego błędu jest prawidłowa.
        Assert.Equal("Purchase Order must be exactly 10 characters long.", validationErrors[0]);


    }
}
