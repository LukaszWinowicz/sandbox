using KERP.Application.Abstractions.CQRS;
using KERP.Application.Common.Context;
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

    public RequestPurchaseOrderReceiptDateUpdateCommandHandler(
        ICurrentUserContext currentUserContext,
        IReceiptDateUpdateRequestRepository updateRepository,
        IUnitOfWork unitOfWork)
    {
        _currentUserContext = currentUserContext;
        _updateRepository = updateRepository;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public async Task<Result<bool>> HandleAsync(RequestPurchaseOrderReceiptDateUpdateCommand command, CancellationToken cancellationToken)
    {
        // 📌 Walidacja wejściowa (opcjonalna)
        // if (command.OrderLines is null || !command.OrderLines.Any())
        //     throw new InvalidOperationException("Brak linii do aktualizacji.");

        var allErrors = new List<string>();
        var validEntities = new List<PurchaseOrderReceiptDateUpdateRequestEntity>();

        string userId = _currentUserContext.UserId;
        int factoryId = _currentUserContext.FactoryId;

        foreach (var line in command.OrderLines)
        {
            var entityResult = PurchaseOrderReceiptDateUpdateRequestEntity.Create(
                line.PurchaseOrder,
                line.LineNumber,
                line.Sequence,
                line.NewReceiptDate,
                command.DateType,
                factoryId,
                userId
            );

            if (entityResult.IsFailure)
            {
                allErrors.AddRange(entityResult.Errors);
            }
            else
            {
                validEntities.Add(entityResult.Value!);
            }
        }

        if (allErrors.Any())
        {
            // Jeśli wystąpił jakikolwiek błąd, nie zapisujemy nic i zwracamy wszystkie błędy.
            return Result<bool>.Failure(allErrors.Distinct().ToList());
        }

        // Jeśli wszystko jest w porządku, dodajemy wszystkie encje i zapisujemy.
        foreach (var entity in validEntities)
        {
            await _updateRepository.AddAsync(entity, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}