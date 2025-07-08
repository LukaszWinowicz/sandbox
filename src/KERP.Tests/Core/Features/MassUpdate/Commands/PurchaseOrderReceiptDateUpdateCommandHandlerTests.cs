using KERP.Core.Features.MassUpdate.Commands;
using KERP.Core.Features.MassUpdate.Entities;
using KERP.Core.Features.MassUpdate.Enums;
using KERP.Core.Interfaces.Repositories;
using KERP.Core.Interfaces.Services;
using KERP.Core.Interfaces.ValidationStrategies;
using Moq;
using System;

namespace KERP.Tests.Core.Features.MassUpdate.Commands;

/// <summary>
/// Unit tests for the PurchaseOrderReceiptDateUpdateCommandHandler.
/// </summary>
public class PurchaseOrderReceiptDateUpdateCommandHandlerTests
{
    private readonly Mock<IValidationStrategy<PurchaseOrderReceiptDateUpdateCommand>> _mockValidationStrategy;
    private readonly Mock<IRepository<PurchaseOrderReceiptDateUpdateEntity>> _mockRepository;
    private readonly Mock<IUserService> _mockUserService;
    private readonly PurchaseOrderReceiptDateUpdateCommandHandler _handler;

    public PurchaseOrderReceiptDateUpdateCommandHandlerTests()
    {
        // Inicjalizujemy wszystkie potrzebne mocki
        _mockValidationStrategy = new Mock<IValidationStrategy<PurchaseOrderReceiptDateUpdateCommand>>();
        _mockRepository = new Mock<IRepository<PurchaseOrderReceiptDateUpdateEntity>>();
        _mockUserService = new Mock<IUserService>();

        // Tworzymy prawdziwą instancję handlera, wstrzykując mu nasze mocki
        _handler = new PurchaseOrderReceiptDateUpdateCommandHandler(
            _mockValidationStrategy.Object,
            _mockRepository.Object,
            _mockUserService.Object
        );
    }

    private PurchaseOrderReceiptDateUpdateCommand ValidCommand() => new()
    {
        PurchaseOrder = "PO12345678",
        LineNumber = 10,
        Sequence = 1,
        ReceiptDate = DateTime.Now.AddDays(1),
        DateType = ReceiptDateUpdateType.Confirmed
    };

    /// <summary>
    /// Verifies that the handler returns validation errors immediately
    /// if the validation strategy fails, without attempting to save data.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldReturnValidationErrors_WhenValidationFails()
    {
        // ARRANGE
        // 1. Przygotowanie polecenia z danymi, które spowodują błąd walidacji.
        var command = ValidCommand();
        command.PurchaseOrder = "PO123";       

        // 2. Zdefiniowanie, jakie błędy ma zwrócić mock walidacji.
        var expectedErrors = new List<string> { "PO number is too short." };

        // 3. "Uczenie" mocka walidacji, aby zwrócił błędy.
        _mockValidationStrategy
            .Setup(s => s.ValidateAsync(command))
            .ReturnsAsync(expectedErrors);

        // ACT
        // 4. Wywołanie metody Handle na handlerze z przygotowanym poleceniem.
        var result = await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        // 5. Sprawdzenie, czy handler zwrócił dokładnie te błędy, które dostał od strategii.
        Assert.Equal(expectedErrors, result);

        // 6.KLUCZOWE: Weryfikacja, że repozytorium do zapisu NIGDY nie zostało wywołane.
        _mockRepository.Verify(r => r.AddRangeAsync(It.IsAny<IEnumerable<PurchaseOrderReceiptDateUpdateEntity>>()), Times.Never);
        _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldSaveChanges_WhenValidationSucceeds()
    {
        // ARRANGE
        // 1. Przygotowanie polecenia z poprawnymi danymi.
        var command = ValidCommand();

        // 2. Nauczenie mocka walidacji, aby zwrócił pustą listę błędów (czyli walidacja przeszła).
        _mockValidationStrategy
            .Setup(s => s.ValidateAsync(command))
            .ReturnsAsync(new List<string>());

        // 3. Konfiguracja serwisu użytkownika, aby zwracał jakieś ID.
        _mockUserService
            .Setup(s => s.GetCurrentUserId())
            .Returns("test-user-id");

        // ACT
        // 4. Wywołanie metody Handle na handlerze z przygotowanym poleceniem.
        var result = await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        // 5. Sprawdzenie, czy operacja zakończyła się sukcesem (brak błędów).
        Assert.Empty(result);

        // 6. KLUCZOWE: Weryfikujemy, czy metody zapisu na repozytorium zostały wywołane DOKŁADNIE RAZ.
        // To jest dowód na to, że handler poprawnie kontynuował proces po pomyślnej walidacji.
        _mockRepository.Verify(r => r.AddRangeAsync(It.IsAny<IEnumerable<PurchaseOrderReceiptDateUpdateEntity>>()), Times.Once);
        _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Theory]
    [InlineData(null, "Purchase order cannot be null.")]
    [InlineData("", "Purchase order cannot be empty.")]
    [InlineData("PO", "Purchase order is too short.")]
    public async Task Handle_ShouldReturnValidationErrors_ForInvalidPurchaseOrders(string inputPO, string expectedError)
    {
        // ARRANGE
        var command = ValidCommand();
        command.PurchaseOrder = inputPO;

        _mockValidationStrategy
        .Setup(v => v.ValidateAsync(command))
        .ReturnsAsync(new List<string> { expectedError });

        // ACT
        var result = await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        Assert.Single(result);
        Assert.Equal(expectedError, result.First());
        _mockRepository.Verify(r => r.AddRangeAsync(It.IsAny<IEnumerable<PurchaseOrderReceiptDateUpdateEntity>>()), Times.Never);
        _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Never);
    }
}
