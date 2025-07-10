using KERP.Application.Interfaces;
using KERP.Domain.Entities.MassUpdate.PurchaseOrder;
using KERP.Domain.Interfaces.MassUpdate.PurchaseOrder;
using KERP.Domain.Interfaces.Shared;

namespace KERP.Application.MassUpdate.PurchaseOrder.Commands;

// W przyszłości ten interfejs będzie pochodził z naszego DiyMediator
public interface ICommandHandler<TCommand>
{
    Task Handle(TCommand command, CancellationToken cancellationToken);
}

public class PurchaseOrderReceiptDateUpdateCommandHandler : ICommandHandler<PurchaseOrderReceiptDateUpdateCommand>
{
    private readonly IPurchaseOrderReceiptDateUpdateRepository _updateRepository;
    private readonly IFactoryRepository _factoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public PurchaseOrderReceiptDateUpdateCommandHandler(
    IPurchaseOrderReceiptDateUpdateRepository updateRepository,
    IFactoryRepository factoryRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService)
    {
        _updateRepository = updateRepository;
        _factoryRepository = factoryRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task Handle(PurchaseOrderReceiptDateUpdateCommand command, CancellationToken cancellationToken)
    {
        // Pobieramy ID użytkownika i fabryki z serwisu
        var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException("User is not authenticated.");
        var factoryId = _currentUserService.SelectedFactoryId ?? throw new InvalidOperationException("No factory selected.");

        var factory = await _factoryRepository.GetByIdAsync(factoryId, cancellationToken);
        if (factory is null)
        {
            throw new InvalidOperationException($"Factory with ID {factoryId} not found.");
        }

        // TODO: W tym miejscu w przyszłości uruchomimy nasz łańcuch walidacji!

        foreach (var orderDto in command.OrdersToUpdate)
        {
            var newUpdateEntity = new PurchaseOrderReceiptDateUpdateEntity
            {
                PurchaseOrder = orderDto.PurchaseOrder,
                LineNumber = orderDto.LineNumber,
                Sequence = orderDto.Sequence,
                ReceiptDate = orderDto.ReceiptDate,
                DateType = orderDto.DateType,
                FactoryId = factory.Id,   // Możemy użyć ID z pobranego obiektu
                Factory = factory,        // <-- A TUTAJ PRZYPISUJEMY CAŁY OBIEKT
                UserId = userId,       // Używamy ID z kontekstu
                AddedDate = DateTime.UtcNow
            };

            // Dodajemy encję do "koszyka" zmian (DbContext)
            await _updateRepository.AddAsync(newUpdateEntity, cancellationToken);
        }

        // Zapisujemy wszystkie zmiany z "koszyka" do bazy danych w jednej transakcji
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
