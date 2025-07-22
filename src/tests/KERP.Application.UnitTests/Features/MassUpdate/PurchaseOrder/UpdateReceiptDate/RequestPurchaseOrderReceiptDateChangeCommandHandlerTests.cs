using FluentAssertions;
using KERP.Application.Common.Abstractions;
using KERP.Application.Features.MassUpdates.PurchaseOrder.UpdateReceiptDate;
using KERP.Application.Services;
using KERP.Domain.Aggregates.PurchaseOrder;
using Moq;

namespace KERP.Application.UnitTests.Features.MassUpdate.PurchaseOrder.UpdateReceiptDate;

public class RequestPurchaseOrderReceiptDateChangeCommandHandlerTests
{
    // Tworzymy "atrapy" (mocki) naszych zależności
    private readonly Mock<IPurchaseOrderReceiptDateChangeRequestRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;

    // To jest nasz SUT - System Under Test (System Testowany)
    private readonly RequestPurchaseOrderReceiptDateChangeCommandHandler _handler;

    public RequestPurchaseOrderReceiptDateChangeCommandHandlerTests()
    {
        // Inicjalizujemy mocki przed każdym testem
        _repositoryMock = new Mock<IPurchaseOrderReceiptDateChangeRequestRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();

        // Tworzymy prawdziwą instancję handlera, wstrzykując do niego nasze atrapy
        _handler = new RequestPurchaseOrderReceiptDateChangeCommandHandler(
            _repositoryMock.Object,
            _unitOfWorkMock.Object,
            _currentUserServiceMock.Object);
    }

    [Fact]
    public async Task Handle_WhenCommandIsValid_ShouldCreateRequestsAndSaveChanges()
    {
        // Arrange (Przygotuj)
        // 1. Tworzymy komendę z dwoma przykładowymi wierszami
        var command = new RequestPurchaseOrderReceiptDateChangeCommand
        {
            DateType = ReceiptDateUpdateType.Confirmed,
            OrderLines = new List<OrderLineDto>
        {
            new("PO-01", 10, 1, DateTime.Now),
            new("PO-02", 20, 1, DateTime.Now)
        }
        };

        // 2. Konfigurujemy nasze mocki, aby zachowywały się tak, jak tego oczekujemy
        _currentUserServiceMock.Setup(s => s.UserId).Returns("test-user-id");
        _currentUserServiceMock.Setup(s => s.FactoryId).Returns(100);

        // Act (Działaj)
        // 3. Wywołujemy metodę, którą testujemy
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert (Sprawdź)
        // 4. Weryfikujemy, czy wynik operacji jest poprawny
        result.IsSuccess.Should().BeTrue();

        // 5. Weryfikujemy, czy na naszych mockach zostały wywołane odpowiednie metody
        _repositoryMock.Verify(
            r => r.Add(It.IsAny<PurchaseOrderReceiptDateChangeRequest>()),
            Times.Exactly(2)); // Sprawdzamy, czy Add zostało wywołane 2 razy (dla 2 wierszy)

        _unitOfWorkMock.Verify(
            uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once); // Sprawdzamy, czy SaveChangesAsync zostało wywołane dokładnie 1 raz
    }
}
