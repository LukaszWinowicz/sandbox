using KERP.Core.Abstractions.Messaging;
using KERP.Core.Features.MassUpdate.Entities;
using KERP.Core.Interfaces.Repositories;
using KERP.Core.Interfaces.Services;
using KERP.Core.Interfaces.ValidationStrategies;

namespace KERP.Core.Features.MassUpdate.Commands;

/// <summary>
/// Handles the <see cref="PurchaseOrderReceiptDateUpdateCommand"/>.
/// </summary>
public class PurchaseOrderReceiptDateUpdateCommandHandler
       : IRequestHandler<PurchaseOrderReceiptDateUpdateCommand, List<string>>
{
    private readonly IValidationStrategy<PurchaseOrderReceiptDateUpdateCommand> _validationStrategy;
    private readonly IRepository<PurchaseOrderReceiptDateUpdateEntity> _repository;
    private readonly IUserService _userService;

    // Usunęliśmy zależność od IUserService
    public PurchaseOrderReceiptDateUpdateCommandHandler(
        IValidationStrategy<PurchaseOrderReceiptDateUpdateCommand> validationStrategy,
        IRepository<PurchaseOrderReceiptDateUpdateEntity> repository,
        IUserService userService)
    {
        _validationStrategy = validationStrategy;
        _repository = repository;
        _userService = userService;
    }

    /// <summary>
    /// Handles the command by validating the input, mapping it to an entity, and persisting it.
    /// </summary>
    public async Task<List<string>> Handle(PurchaseOrderReceiptDateUpdateCommand command, CancellationToken cancellationToken)
    {
        // --- Krok 1: Walidacja ---
        var validationErrors = await _validationStrategy.ValidateAsync(command);
        if (validationErrors.Any())
        {
            return validationErrors;
        }

        // --- Krok 2: Mapowanie (z Command na Encję) ---
        var entity = new PurchaseOrderReceiptDateUpdateEntity
        {
            PurchaseOrder = command.PurchaseOrder,
            LineNumber = command.LineNumber,
            Sequence = command.Sequence,
            ReceiptDate = command.ReceiptDate.Value,
            DateType = command.DateType,
            UserId = _userService.GetCurrentUserId() ?? "system",

        };

        // --- Krok 3: Zapis do Bazy Danych ---
        await _repository.AddRangeAsync(new[] { entity });
        await _repository.SaveChangesAsync();

        // --- Krok 4: Sukces ---
        return new List<string>();
    }
}