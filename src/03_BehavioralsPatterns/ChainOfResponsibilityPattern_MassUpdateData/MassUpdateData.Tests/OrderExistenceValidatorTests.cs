using MassUpdateData.Models;
using MassUpdateData.Services;
using MassUpdateData.Validators;
using Moq;

namespace MassUpdateData.Tests;

public class OrderExistenceValidatorTests
{
    [Fact]
    // MetodaTestowana_OczekiwanyRezultat_Scenariusz
    public void Validate_ShouldAddError_WhenOrderDoesNotExist()
    {
        // ARRANGE
        // 1. Stworzenie obiektu żądania do testu
        var request = new UpdateRequest { Order = "NIEISTNIEJACY" };

        // 2. Stworzenie mock (zaślepki) serwisu danych
        var mockDataService = new Mock<IOrderDataService>();

        // 3. Nauka mocka, jak ma się zachowywać: gdy ktoś zapyta o "NIEISTNIEJACY"
        mockDataService.Setup(service => service.OrderExists("NIEISTNIEJACY")).Returns(false);

        // 4. Stworzenie instacji testowanego walidatora, wstrzykując mu mock, a nie prawdziwy serwis
        var validator = new OrderExistenceValidator(mockDataService.Object);

        // 5. Przygotuj dokładny, oczekiwany komunikat
        string expectedError = "ERROR: Order 'NIEISTNIEJACY' does not exist in the database.";


        // ACT
        // Wywołanie metody testowanej
        validator.Validate(request);

        // ASSERT
        // Sprawdzenie czy wynik jest zgodny z oczekiwaniami
        Assert.Single(request.ValidationErrors); // Oczekujemy dokładnie jednego błędu
        Assert.Equal(expectedError, request.ValidationErrors[0]); // Sprawdź treść błędu
    }
}