using MassUpdate.Core.DTOs;
using MassUpdate.Core.Enums;
using MassUpdate.Core.Handlers;
using MassUpdate.Core.Interfaces.Repositories;
using MassUpdate.Core.Validators.Components;
using Moq;

namespace MassUpdateData.Tests.UnitTests;

/// <summary>
/// Testy jednostkowe dla generycznego komponentu walidacyjnego ExistenceValidator.
/// </summary>
public class ExistenceValidatorTests
{
    // Prywatne pola dla obiektów używanych we wszystkich testach.
    private readonly Mock<IPurchaseOrderValidationRepository> _mockRepo;
    private readonly Mock<IValidationHandler> _mockNextHandler;
    private readonly UpdateReceiptDateDto _testDto;

    /// <summary>
    /// Konstruktor klasy testowej. Inicjalizuje wspólne obiekty
    /// używane we wszystkich scenariuszach testowych w tej klasie, zgodnie z zasadą DRY.
    /// </summary>
    public ExistenceValidatorTests()
    {
        // Inicjalizacja wspólnych mocków i danych testowych.
        _mockRepo = new Mock<IPurchaseOrderValidationRepository>();
        _mockNextHandler = new Mock<IValidationHandler>();
        _testDto = new UpdateReceiptDateDto
        {
            PurchaseOrder = "DUMMY_PO",
            LineNumber = 10,
            Sequence = 1,
            ReceiptDate = DateTime.Now,
            DateType = ReceiptDateUpdateType.Confirmed
        };
    }

    /// <summary>
    /// Weryfikuje, że walidator poprawnie dodaje błąd,
    /// gdy przekazana funkcja sprawdzająca istnienie zwróci `false`.
    /// </summary>
    [Fact]
    public async Task ValidateAsync_ShouldAddError_WhenEntityDoesNotExist()
    {
        // ARRANGE
        // Konfigurujemy ZACHOWANIE mocka specyficzne dla tego testu.
        _mockRepo.Setup(repo => repo.OrderExistsAsync(_testDto.PurchaseOrder)).ReturnsAsync(false);

        // Tworzymy instancję walidatora, którego testujemy.
        var validator = new ExistenceValidator<string>(
            dto => ((UpdateReceiptDateDto)dto).PurchaseOrder,
            _mockRepo.Object.OrderExistsAsync,
            "Purchase Order"
        );

        // Tworzymy obiekt żądania walidacji.
        var request = new ValidationRequest(_testDto);

        // ACT
        // Wywołujemy główną metodę walidacyjną.
        await validator.ValidateAsync(request);

        // ASSERT
        // Sprawdzamy, czy na liście jest dokładnie jeden błąd i czy ma poprawną treść.
        Assert.Single(request.ValidationErrors);
        Assert.Contains("does not exist", request.ValidationErrors[0]);

    }

    /// <summary>
    /// Weryfikuje, że walidator nie dodaje błędu i przekazuje żądanie dalej,
    /// gdy przekazana funkcja sprawdzająca istnienie zwróci `true`.
    /// </summary>
    [Fact]
    public async Task Validate_ShouldNotAddError_AndShouldCallNext_WhenEntityExists()
    {
        // ARRANGE
        // Konfigurujemy ZACHOWANIE mocka specyficzne dla tego testu.
        _mockRepo.Setup(repo => repo.OrderExistsAsync(_testDto.PurchaseOrder)).ReturnsAsync(true);

        // Tworzymy instancję walidatora, którego testujemy.
        var validator = new ExistenceValidator<string>(
            dto => ((UpdateReceiptDateDto)dto).PurchaseOrder,
            _mockRepo.Object.OrderExistsAsync,
            "Purchase Order"
        );

        // Ustawiamy "kolejne ogniwo" w łańcuchu, aby sprawdzić, czy zostanie wywołane.
        validator.SetNext(_mockNextHandler.Object);

        // Tworzymy obiekt żądania walidacji.
        var request = new ValidationRequest(_testDto);

        // ACT
        // Wywołujemy główną metodę walidacyjną.
        await validator.ValidateAsync(request);

        // ASSERT
        // Sprawdzamy, czy lista błędów jest pusta.
        Assert.Empty(request.ValidationErrors);

        // Weryfikujemy, czy następne ogniwo w łańcuchu zostało wywołane.
        _mockNextHandler.Verify(handler => handler.ValidateAsync(request), Times.Once);

    }
}