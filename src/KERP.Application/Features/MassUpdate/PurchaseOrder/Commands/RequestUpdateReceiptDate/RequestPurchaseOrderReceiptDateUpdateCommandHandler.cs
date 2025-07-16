using KERP.Application.Abstractions.CQRS;
using KERP.Application.Common.Context;
using KERP.Application.Shared.Validators;
using KERP.Domain.Abstractions;
using KERP.Domain.Abstractions.Repositories.MassUpdate.PurchaseOrder;
using KERP.Domain.Abstractions.Results;
using KERP.Domain.Entities.MassUpdate.PurchaseOrder;

namespace KERP.Application.Features.MassUpdate.PurchaseOrder.Commands.RequestUpdateReceiptDate;

/// <summary>
/// Handler obsługujący komendę <see cref="RequestPurchaseOrderReceiptDateUpdateCommand"/>.
/// Tworzy wpisy aktualizacji dat przyjęcia dla wskazanych pozycji zamówień.
/// </summary>
public sealed class RequestPurchaseOrderReceiptDateUpdateCommandHandler : ICommandHandler<RequestPurchaseOrderReceiptDateUpdateCommand, Result<bool>>
{
    private readonly ICurrentUserContext _currentUserContext;
    private readonly IReceiptDateUpdateRequestRepository _updateRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly PurchaseOrderReceiptDateUpdateValidator _validator;

    public RequestPurchaseOrderReceiptDateUpdateCommandHandler(
        ICurrentUserContext currentUserContext,
        IReceiptDateUpdateRequestRepository updateRepository,
        IUnitOfWork unitOfWork,
        PurchaseOrderReceiptDateUpdateValidator validator)
    {
        _currentUserContext = currentUserContext;
        _updateRepository = updateRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    /// <inheritdoc />
    public async Task<Result<bool>> HandleAsync(RequestPurchaseOrderReceiptDateUpdateCommand command, CancellationToken cancellationToken)
    {
        // Walidacja wejściowa – wszystkie błędy zbieramy i zwracamy do UI
        var validationErrors = await _validator.ValidateAsync(command);
        if (validationErrors.Any())
            return Result<bool>.Failure(validationErrors);

        var validEntities = new List<PurchaseOrderReceiptDateUpdateRequestEntity>();

        string userId = _currentUserContext.UserId;
        int factoryId = _currentUserContext.FactoryId;

        foreach (var line in command.OrderLines)
        {
            // Tworzymy encję – zakładamy, że dane już są poprawne (brak walidacji technicznej)
            var entity = PurchaseOrderReceiptDateUpdateRequestEntity.Create(
                line.PurchaseOrder,
                line.LineNumber,
                line.Sequence,
                line.NewReceiptDate,
                command.DateType,
                factoryId,
                userId
            );
            validEntities.Add(entity);
        }

        // Dodajemy encje i zapisujemy do bazy
        foreach (var entity in validEntities)
        {
            await _updateRepository.AddAsync(entity, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}