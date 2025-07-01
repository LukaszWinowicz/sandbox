using MassUpdate.Core.DTOs;
using MassUpdate.Core.Enums;
using MassUpdate.Core.Handlers;
using MassUpdate.Core.Interfaces.Repositories;
using MassUpdate.Core.Services;
using MassUpdate.Core.Validators.Components;
using Moq;

namespace MassUpdateData.Tests.UnitTests;

public class CombinationValidatorTests
{
    // Prywatne pola do obiektów używanych we wszystkich testach
    private readonly Mock<IPurchaseOrderValidationRepository> _mockRepo;
    private readonly Mock<IValidationHandler> _mockNextHandler;
    private readonly UpdateReceiptDateDto _testDto;

    /// <summary>
    /// Konstruktor klasy testowej. Inicjalizuje wspólne obiekty
    /// używane we wszystkich scenariuszach testowych w tej klasie, zgodnie z zasadą DRY.
    /// </summary>
    public CombinationValidatorTests()
    {
        // Inicjalizacja wspólnych mocków i danych testowych
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

    [Fact]
    public async Task ValidateAsync_ShouldAddError_WhenCombinationCheckReturnsFalse()
    {
        // ARRANGE
        // Konfigurujemy ZACHOWANIE mocka specyficzne dla tego testu.
        _mockRepo.Setup(repo => repo.CombinationExistsAsync(
            _testDto.PurchaseOrder, _testDto.LineNumber, _testDto.Sequence))
            .ReturnsAsync(false);

        // Tworzymy "przepis", który będzie wywoływać nasz zamockowany serwis.
        Func<UpdateReceiptDateDto, IPurchaseOrderValidationRepository, Task<bool>> checkFunc =
               (dto, repo) => repo.CombinationExistsAsync(dto.PurchaseOrder, dto.LineNumber, dto.Sequence);

        // Tworzymy instancję walidatora, ktrego testujemy.
        var validator = new CombinationValidator<UpdateReceiptDateDto, IPurchaseOrderValidationRepository>(
                        _mockRepo.Object,
                        checkFunc,
                        "Combination does not exist."
                    );

        // Tworzymy obiekt żądania walidacji.
        var request = new ValidationRequest(_testDto);

        // ACT
        // Wywołujemy główną metodę walidacyjną.
            await validator.ValidateAsync(request);

        // ASSERT
        // Sprawdzamy, czy na liście jest dokładnie jeden błąd i czy ma poprawną treść.
        Assert.Single(request.ValidationErrors);
        Assert.Equal("Combination does not exist.", request.ValidationErrors[0]);
    }

    /// <summary>
    /// Weryfikuje, że walidator nie dodaje błędu i przekazuje żądanie dalej,
    /// gdy przekazany "przepis na walidację" zwróci `true`.
    /// </summary>
    [Fact]
    public async Task ValidateAsync_ShouldNotAddError_WhenCombinationSucceeds()
    {
        // ARRANGE
        // Konfigurujemy ZACHOWANIE mocka specyficzne dla tego testu.
        _mockRepo.Setup(repo => repo.CombinationExistsAsync(
           _testDto.PurchaseOrder, _testDto.LineNumber, _testDto.Sequence))
           .ReturnsAsync(true);

        // Tworzymy "przepis", który będzie wywoływać nasz zamockowany serwis.
        Func<UpdateReceiptDateDto, IPurchaseOrderValidationRepository, Task<bool>> checkFunc =
               (dto, repo) => repo.CombinationExistsAsync(dto.PurchaseOrder, dto.LineNumber, dto.Sequence);

        // Tworzymy instancję walidatora, ktrego testujemy.
        var validator = new CombinationValidator<UpdateReceiptDateDto, IPurchaseOrderValidationRepository>(
                        _mockRepo.Object,
                        checkFunc,
                        "Combination does not exist."
                    );

        // Ustawiamy "kolejne ogniwo" w łańcuchu.
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