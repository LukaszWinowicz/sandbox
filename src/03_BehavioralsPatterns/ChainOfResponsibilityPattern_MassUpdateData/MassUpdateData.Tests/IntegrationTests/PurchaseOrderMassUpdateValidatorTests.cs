using MassUpdateData.Models;
using MassUpdateData.Services;
using MassUpdateData.Validators;
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
            ReceiptDate = DateTime.Now.AddDays(1),
        };

        // ACT
        // Wywołujemy główną metodę walidującą.
        List<string> validationErrors = mainValidator.Validate(validDto);

        // ASSERT
        // Sprawdzamy, czy rezultat jest zgodny z naszymi oczekiwaniami.
        // Dla "Happy Path" lista błędów powinna być pusta.
        Assert.Empty(validationErrors);
    }
}
