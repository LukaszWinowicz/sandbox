using KERP.Application.Abstractions.CQRS;
using KERP.Application.Common.Context;
using KERP.Domain.Abstractions;
using KERP.Domain.Abstractions.Repositories.MassUpdate.PurchaseOrder;
using KERP.Domain.Entities.MassUpdate.PurchaseOrder;

namespace KERP.Application.Features.MassUpdate.PurchaseOrder.Commands.RequestUpdateReceiptDate;

/// <summary>
/// Handler obsługujący komendę <see cref="RequestPurchaseOrderReceiptDateUpdateCommand"/>.
/// Tworzy wpisy aktualizacji dat przyjęcia dla wskazanych pozycji zamówień.
/// </summary>
public sealed class RequestPurchaseOrderReceiptDateUpdateCommandHandler : ICommandHandler<RequestPurchaseOrderReceiptDateUpdateCommand>
{
    private readonly ICurrentUserContext _currentUserContext;
    private readonly IPurchaseOrderReceiptDateUpdateRepository _updateRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RequestPurchaseOrderReceiptDateUpdateCommandHandler(
        ICurrentUserContext currentUserContext,
        IPurchaseOrderReceiptDateUpdateRepository updateRepository,
        IUnitOfWork unitOfWork)
    {
        _currentUserContext = currentUserContext;
        _updateRepository = updateRepository;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public async Task HandleAsync(RequestPurchaseOrderReceiptDateUpdateCommand command, CancellationToken cancellationToken)
    {
        // 📌 Walidacja wejściowa (opcjonalna)
        // if (command.OrderLines is null || !command.OrderLines.Any())
        //     throw new InvalidOperationException("Brak linii do aktualizacji.");

        // KROK 1: Pobranie danych o użytkowniku
        string userId = _currentUserContext.UserId;
        int factoryId = _currentUserContext.FactoryId;

        // KROK 2: Przetwarzanie każdej linii i tworzenie encji
        foreach (var line in command.OrderLines)
        {
            var updateEntity = PurchaseOrderReceiptDateUpdateRequestEntity.Create(
                line.PurchaseOrder,
                line.LineNumber,
                line.Sequence,
                line.NewReceiptDate,
                command.DateType,
                factoryId,
                userId
            );

            // KROK 3: Dodanie encji do repozytorium
            await _updateRepository.AddAsync(updateEntity, cancellationToken);
        }

        // KROK 4: Zapis zmian (jedna transakcja)
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}