using KERP.Application.Interfaces;
using KERP.Application.MassUpdate.PurchaseOrder.Validation;
using KERP.Application.Shared.Exceptions;
using KERP.Application.Shared.Mediator;
using KERP.Domain.Entities.MassUpdate.PurchaseOrder;
using KERP.Domain.Interfaces.MassUpdate.PurchaseOrder;
using KERP.Domain.Interfaces.Shared;

namespace KERP.Application.MassUpdate.PurchaseOrder.Commands;


public class PurchaseOrderReceiptDateUpdateCommandHandler : ICommandHandler<PurchaseOrderReceiptDateUpdateCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IFactoryRepository _factoryRepository;
    private readonly IPurchaseOrderReceiptDateUpdateRepository _updateRepository;
    private readonly IReceiptDateUpdateValidator _validator;

    public PurchaseOrderReceiptDateUpdateCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IFactoryRepository factoryRepository,
        IPurchaseOrderReceiptDateUpdateRepository updateRepository,
        IReceiptDateUpdateValidator validator)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _factoryRepository = factoryRepository;
        _updateRepository = updateRepository;
        _validator = validator;
    }

    public async Task Handle(PurchaseOrderReceiptDateUpdateCommand command, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
        var factoryId = _currentUserService.SelectedFactoryId ?? throw new InvalidOperationException("No factory selected.");

        // KROK 1: Usuwamy pobieranie obiektu Factory. Nie jest już potrzebne.
        // var factory = await _factoryRepository.GetByIdAsync(factoryId, cancellationToken) 
        //    ?? throw new InvalidOperationException($"Factory with ID {factoryId} not found.");

        var allErrors = new List<string>();
        int rowNumber = 1;

        foreach (var orderDto in command.OrdersToUpdate)
        {
            var validationErrors = await _validator.ValidateAsync(orderDto);
            if (validationErrors.Any())
            {
                allErrors.AddRange(validationErrors.Select(e => $"Row {rowNumber}: {e}"));
            }
            rowNumber++;
        }

        if (allErrors.Any())
        {
            throw new ValidationException(allErrors);
        }

        foreach (var orderDto in command.OrdersToUpdate)
        {
            var newUpdateEntity = new PurchaseOrderReceiptDateUpdateEntity
            {
                PurchaseOrder = orderDto.PurchaseOrder,
                LineNumber = orderDto.LineNumber,
                Sequence = orderDto.Sequence,
                ReceiptDate = orderDto.ReceiptDate,
                DateType = orderDto.DateType,
                UserId = userId,
                AddedDate = DateTime.UtcNow,
                IsProcessed = false,
                ProcessedDate = null,

                // KROK 2: Ustawiamy TYLKO klucz obcy. Właściwość nawigacyjna zostaje pusta.
                FactoryId = factoryId
                // Usunęliśmy: Factory = factory 
            };
            await _updateRepository.AddAsync(newUpdateEntity, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}