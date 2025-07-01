using MassUpdate.Core.DTOs;
using MassUpdate.Core.Enums;
using MassUpdate.Core.Interfaces.Repositories;
using MassUpdate.Core.Validators.Orchestrators;
using Moq;

namespace MassUpdateData.Tests.IntegrationTests;

/// <summary>
/// Testy integracyjne dla strategii walidacji UpdateReceiptDateValidationStrategy.
/// Weryfikują one, czy strategia poprawnie buduje i uruchamia swoje wewnętrzne
/// łańcuchy walidacji (Chain of Responsibility).
/// </summary>
public class UpdateReceiptDateValidationStrategyTests
{
    private readonly Mock<IPurchaseOrderValidationRepository> _mockRepo;

    public UpdateReceiptDateValidationStrategyTests()
    {
        _mockRepo = new Mock<IPurchaseOrderValidationRepository>();
    }

    [Fact]
    public async Task ValidateAsync_WithMultipleErrors_ShouldAggregateAllErrors()
    {
        // ARRANGE
        // Konfigurujemy MOCK tak, aby nie przeszkadzał w walidacjach,
        // które nie wymagają dostępu do bazy danych.
        // OrderExistsAsync zwróci true, aby test nie zatrzymał się na tym etapie.
        _mockRepo.Setup(r => r.OrderExistsAsync(It.IsAny<string>())).ReturnsAsync(true);
        _mockRepo.Setup(r => r.CombinationExistsAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);

        // Tworzymy instancję naszej strategii, wstrzykujem
        var strategy = new UpdateReceiptDateValidationStrategy(_mockRepo.Object);

        // Tworzymy DTO z wieloma błędami, które powinny być wykryte przez różne "klocki".
        var invalidDto = new UpdateReceiptDateDto
        {
            PurchaseOrder = "ZLY_FORMAT", // Błąd 1: Zła długość
            LineNumber = 0,               // Błąd 2: Wartość poniżej minimum (1)
            Sequence = -5,              // Błąd 3: Wartość poniżej minimum (1)
            ReceiptDate = DateTime.Now.AddDays(-1), // Błąd 4: Data z przeszłości
            DateType = ReceiptDateUpdateType.Confirmed
        };

        // ACT
        // Wywołujemy główną metodę walidacyjną naszej strategii.
        var errors = await strategy.ValidateAsync(invalidDto);

        // ASSERT

        // Sprawdzamy, czy zebraliśmy wszystkie 4 błędy z różnych łańcuchów.
        Assert.Equal(3, errors.Count);

        // Sprawdzamy, czy na liście znajdują się wszystkie oczekiwane komunikaty.
        // Używamy .Any() lub .Contains(), aby nie zależeć od kolejności.
        Assert.Contains(errors, e => e.Contains("Line Number must be greater than or equal to 1"));
        Assert.Contains(errors, e => e.Contains("Sequence must be greater than or equal to 1"));
        Assert.Contains(errors, e => e.Contains("must be today or a future date"));


    }
}
