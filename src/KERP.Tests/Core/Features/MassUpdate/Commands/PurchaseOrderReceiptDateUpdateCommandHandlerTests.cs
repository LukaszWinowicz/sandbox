using KERP.Core.Features.MassUpdate.Commands;
using KERP.Core.Features.MassUpdate.Entities;
using KERP.Core.Interfaces.Repositories;
using KERP.Core.Interfaces.Services;
using KERP.Core.Interfaces.ValidationStrategies;
using Moq;

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

}
