using MassUpdateData.Handlers;
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

    [Fact]
    public void Validate_ShouldNotAddError_AndShouldCallNext_WhenEntityExists()
    {
        // ARRANGE
        // 1. Tworzymy zaślepkę (mock) naszego serwisu danych.
        var mockDataService = new Mock<IOrderDataService>();

        // 2. "Uczymy" mocka, jak ma się zachować.
        // Mówimy mu: "Gdy ktoś wywoła na Tobię metodę OrderExist z argumentem 'GOOD_PO', ZAWSZE zwróć `true`"
        mockDataService.Setup(service => service.OrderExists("GOOD_PO")).Returns(true);

        // 3. Tworzymy mocka NASTĘPNEGO ogniwa w łańcuchu, aby sprawdzić, czy zostanie wywołany.
        var mockNextHandler = new Mock<IValidationHandler>();

        // 4. Tworzymy instancję walidatora, którego będziemy testować.
        // Wstrzykujemy mu funkcję sprawdzającą, która pochodzi z naszego mocka i podpinamy do niego zamockowane następne ogniwo.
        var validator = new ExistenceValidator<string>(
            dto => ((MassUpdatePurchaseOrderDto)dto).PurchaseOrder!, 
            mockDataService.Object.OrderExists,
            "Purchase Order");
        validator.SetNext(mockNextHandler.Object);

        // 5. Przygotowujemy dane wejściowe.
        var request = new ValidationRequest(new MassUpdatePurchaseOrderDto { PurchaseOrder = "GOOD_PO" });

        // ACT
        // Wywołujemy metodę, którą testujemy.
        validator.Validate(request);

        // ASSERT
        // 1. Sprawdzamy, czy nie został dodany żaden błąd
        Assert.Empty(request.ValidationErrors);

        // 2. Weryfikujemy, czy metoda Validate() na następnym ogniwie została wywołana dokładnie raz.
        // To potwierdza, że łańcuch jest kontynuowany.
        mockNextHandler.Verify(handler => handler.Validate(request), Times.Once);


    }
}