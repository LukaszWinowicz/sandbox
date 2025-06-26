using MassUpdate.Core.DTOs;
using MassUpdate.Core.Factories;
using MassUpdate.Core.Interfaces;
using MassUpdate.Infrastructure.Services;
using Moq;

namespace MassUpdateData.Tests.IntegrationTests;

public class MassUpdateValidationServiceTests
{
    [Fact]
    public void Validate_WithMixedData_ShouldReturnResultsOnlyForInvalidRows()
    {
        // ARRANGE
        // 1. Definiujemy dane testowe
        var validPoNumber = "PO2025-001";
        var invalidPoNumber = "PO-BAD-NUM";

        var dtoList = new List<MassUpdatePurchaseOrderDto>
        {
            // Wiersz 1: Poprawny
            new() { PurchaseOrder = validPoNumber, LineNumber = 10, Sequence = 1, ReceiptDate = DateTime.Now.AddDays(1) },

            // Wiersz 2: Niepoprawny (nie istnieje w "bazie danych")
            new() { PurchaseOrder = invalidPoNumber, LineNumber = 10, Sequence = 1, ReceiptDate = DateTime.Now.AddDays(1) },

            // Wiersz 3: Poprawny
            new() { PurchaseOrder = validPoNumber, LineNumber = 20, Sequence = 5, ReceiptDate = DateTime.Now.AddDays(1) }

        };

        // 2. Tworzymy mocka serwisu danych
        var mockDataService = new Mock<IOrderDataService>();

        // 3. "Uczymy" mocka, jak ma odpowiadać dla obu przypadków
        mockDataService.Setup(s => s.OrderExists(validPoNumber)).Returns(true);
        mockDataService.Setup(s => s.OrderExists(invalidPoNumber)).Returns(false);

        // Zakładamy, że kombinacje istnieją, aby nie generować dodatkowych błędów
        mockDataService.Setup(s => s.LineCombinationExists(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(true);

        // 4. Tworzymy PRAWDZIWE instancje naszej fabryki i serwisu, wstrzykując im mocka
        var factory = new ValidatorFactory(mockDataService.Object);
        var validationService = new MassUpdateValidationService(factory);

        // ACT
        // 5. Wywołujemy metodę, którą testujemy
        var results = validationService.Validate(dtoList);

        // ASSERT
        // 6. Sprawdzamy, czy otrzymaliśmy wynik tylko dla jednego, niepoprawnego wiersza
        Assert.Single(results);

        // 7. Sprawdzamy, czy szczegóły tego wyniku są poprawne
        var invalidRowResult = results[0];
        Assert.Equal(2, invalidRowResult.RowNumber); // Błąd wystąpił w 2. wierszu
        Assert.Single(invalidRowResult.Errors); // Oczekujemy jednego błędu dla tego wiersza
        Assert.Contains("does not exist", invalidRowResult.Errors[0]); // Sprawdzamy treść błędu
    }
}
