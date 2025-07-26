using KERP.Application.Common.Abstractions;
using KERP.Application.Common.Models;
using KERP.Application.Services;
using KERP.Domain.Aggregates.PurchaseOrder;
using KERP.Domain.Exceptions;

namespace KERP.Application.Features.MassUpdates.PurchaseOrder.UpdateReceiptDate;

/// <summary>
/// Obsługuje komendę tworzenia żądań zmiany daty odbioru dla zamówień zakupu.
/// </summary>
public sealed class RequestPurchaseOrderReceiptDateChangeCommandHandler
    : ICommandHandler<RequestPurchaseOrderReceiptDateChangeCommand, Result<List<RowValidationResult>>>
{
    private readonly IPurchaseOrderReceiptDateChangeRequestRepository _requestRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public RequestPurchaseOrderReceiptDateChangeCommandHandler(
        IPurchaseOrderReceiptDateChangeRequestRepository requestRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _requestRepository = requestRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    /// <summary>
    /// Przetwarza komendę, tworząc wiele agregatów w ramach jednej transakcji.
    /// </summary>
    public async Task<Result<List<RowValidationResult>>> Handle(RequestPurchaseOrderReceiptDateChangeCommand command, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
        var factoryId = _currentUserService.FactoryId ?? throw new BusinessRuleValidationException("Brak kontekstu fabryki dla użytkownika.");

        var results = new List<RowValidationResult>();
        var validRequests = new List<PurchaseOrderReceiptDateChangeRequest>();
        int rowNumber = 1;

        foreach (var line in command.OrderLines)
        {
            try
            {
                var changeRequest = PurchaseOrderReceiptDateChangeRequest.Create(
                    purchaseOrder: line.PurchaseOrder,
                    lineNumber: line.LineNumber,
                    sequence: line.Sequence,
                    newReceiptDate: line.NewReceiptDate,
                    dateType: command.DateType,
                    factoryId: factoryId,
                    userId: userId);

                validRequests.Add(changeRequest);
                results.Add(RowValidationResult.Success(rowNumber));
            }
            catch (BusinessRuleValidationException ex)
            {
                results.Add(RowValidationResult.Failure(rowNumber, ex.Message));
            }
            rowNumber++;
        }

        if (validRequests.Any())
        {
            foreach (var request in validRequests)
            {
                _requestRepository.Add(request);
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return Result<List<RowValidationResult>>.Success(results);
    }
}
