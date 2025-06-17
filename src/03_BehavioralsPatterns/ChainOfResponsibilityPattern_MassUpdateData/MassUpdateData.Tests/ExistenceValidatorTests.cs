using MassUpdateData.Models;
using MassUpdateData.Services;
using MassUpdateData.Validators;
using Moq;
using Newtonsoft.Json.Linq;

namespace MassUpdateData.Tests;

public class ExistenceValidatorTests
{
    [Fact]
    // MetodaTestowana_OczekiwanyRezultat_Scenariusz
    public void Validate_ShouldAddError_WhenEntityDoesNotExist()
    {
        // ARRANGE
        // 1. Tworzymy zaślepkę (mock) naszego serwisu danych.
        var mockDataService = new Mock<IOrderDataService>();

        // 2. "Uczymy" mocka, jak ma się zachować.
        // Mówimy mu: "Gdy ktoś wywoła na Tobię metodę OrderExist z argumentem 'BAD_PO', ZAWSZE zwróć `false`"
        mockDataService.Setup(service => service.OrderExists("BAD_PO")).Returns(false);

        // 3. Tworzymy instancję walidatora, którego będziemy testować.
        // Wstrzykujemy mu funkcję sprawdzającą, która pochodzi z naszego mocka.
        var validator = new ExistenceValidator<string>(
            dto => ((MassUpdatePurchaseOrderDto)dto).PurchaseOrder!,
            mockDataService.Object.OrderExists,
            "Purchase Order"
        );

        // 4. Przygotowujemy dane wejściowe dla testu.
        var request = new ValidationRequest(new MassUpdatePurchaseOrderDto { PurchaseOrder = "BAD_PO" });

        // 5. Przygotowujemy dokładny, oczekiwany komunikat
        string expectedError = "does not exist";

        // ACT
        // Wywołujemy metodę, którą testujemy.
        validator.Validate(request);

        // ASSERT
        // Sprawdzamy, czy rezultat jest zgodny z oczekiwaniami.
        Assert.Single(request.ValidationErrors); // Oczekujemy dokładnie jednego błędu na liście.
        Assert.Contains(expectedError, request.ValidationErrors[0]); // Sprawdzamy, czy komunikat zawiera oczekiwany tekst.
    }
}