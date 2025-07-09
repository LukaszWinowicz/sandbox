

using KERP.Application.Abstractions.Messaging;
using KERP.Application.Interfaces.Services;
using KERP.Domain.Interfaces.Repositories;
using KERP.Domain.MassUpdate.Entities;
using KERP.Domain.Results;

namespace KERP.Application.Features.MassUpdate.Commands;

/// <summary>
/// Handles the <see cref="PurchaseOrderReceiptDateUpdateCommand"/>.
/// </summary>
public class PurchaseOrderReceiptDateUpdateCommandHandler
       : IRequestHandler<PurchaseOrderReceiptDateUpdateCommand, Result<List<RowValidationResult>>>
{
    private readonly IRepository<PurchaseOrderReceiptDateUpdateEntity> _repository;
    private readonly IUserService _userService;

    public PurchaseOrderReceiptDateUpdateCommandHandler(
        IRepository<PurchaseOrderReceiptDateUpdateEntity> repository,
        IUserService userService)
    {
        _repository = repository;
        _userService = userService;
    }

    /// <summary>
    /// Handles the command by validating the input, mapping it to an entity, and persisting it.
    /// </summary>
    public async Task<Result<List<RowValidationResult>>> Handle(PurchaseOrderReceiptDateUpdateCommand command, CancellationToken cancellationToken)
    {
        // --- Krok 1: Mapowanie (z Command na Encję) ---
        var entity = new PurchaseOrderReceiptDateUpdateEntity
        {
            PurchaseOrder = command.PurchaseOrder,
            LineNumber = command.LineNumber,
            Sequence = command.Sequence,
            ReceiptDate = command.ReceiptDate.Value,
            DateType = command.DateType,
            UserId = _userService.GetCurrentUserId() ?? "system",

        };

        // --- Krok 2: Zapis do Bazy Danych ---
        await _repository.AddRangeAsync(new[] { entity });
        await _repository.SaveChangesAsync();

        // --- Krok 3: Sukces ---
        return Result<List<RowValidationResult>>.Success(new List<RowValidationResult>());

    }
}