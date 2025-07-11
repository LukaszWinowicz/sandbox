using KERP.Application.Interfaces;
using KERP.Application.MassUpdate.PurchaseOrder.Validation;
using KERP.Application.Shared.Exceptions;
using KERP.Application.Shared.CQRS;
using KERP.Domain.Entities.MassUpdate.PurchaseOrder;
using KERP.Domain.Interfaces.MassUpdate.PurchaseOrder;
using KERP.Domain.Interfaces.Shared;

namespace KERP.Application.MassUpdate.PurchaseOrder.Commands;


public class PurchaseOrderReceiptDateUpdateCommandHandler : ICommandHandler<PurchaseOrderReceiptDateUpdateCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPurchaseOrderReceiptDateUpdateRepository _updateRepository;
    private readonly IReceiptDateUpdateValidator _validator;

    public PurchaseOrderReceiptDateUpdateCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IPurchaseOrderReceiptDateUpdateRepository updateRepository,
        IReceiptDateUpdateValidator validator)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _updateRepository = updateRepository;
        _validator = validator;
    }

    public async Task Handle(PurchaseOrderReceiptDateUpdateCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
            var factoryId = _currentUserService.SelectedFactoryId ?? throw new InvalidOperationException("No factory selected.");

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
                    FactoryId = factoryId,
                };
                await _updateRepository.AddAsync(newUpdateEntity, cancellationToken);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

        }
        catch (Exception ex)
        {

            // Gdy program się zatrzyma, najedź na 'ex' i rozwiń właściwość 'InnerException'.
            // Zobaczysz tam prawdziwy komunikat błędu od bazy danych.
            Console.WriteLine(ex.ToString()); // To wypisze całą hierarchię wyjątków w konsoli
            throw; // Rzuć wyjątek dalej, aby nie "połknąć" błędu
        }
    }
}