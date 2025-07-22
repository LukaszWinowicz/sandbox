using KERP.Application.Common.Abstractions;
using KERP.Application.Common.Models;
using KERP.Application.Services;
using KERP.Application.Validation;
using KERP.Domain.Aggregates.PurchaseOrder;
using KERP.Domain.Exceptions;

namespace KERP.Application.Features.MassUpdates.PurchaseOrder.UpdateReceiptDate;

/// <summary>
/// Obsługuje komendę tworzenia żądań zmiany daty odbioru dla zamówień zakupu.
/// </summary>
public sealed class RequestPurchaseOrderReceiptDateChangeCommandHandler
    : ICommandHandler<RequestPurchaseOrderReceiptDateChangeCommand, Result>
{
    private readonly IPurchaseOrderReceiptDateChangeRequestRepository _requestRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService; // Zakładamy istnienie serwisu do pobierania danych użytkownika
    private readonly IValidator<RequestPurchaseOrderReceiptDateChangeCommand> _validator;

    public RequestPurchaseOrderReceiptDateChangeCommandHandler(
        IPurchaseOrderReceiptDateChangeRequestRepository requestRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IValidator<RequestPurchaseOrderReceiptDateChangeCommand> validator)
    {
        _requestRepository = requestRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _validator = validator;
    }

    /// <summary>
    /// Przetwarza komendę, tworząc wiele agregatów w ramach jednej transakcji.
    /// </summary>
    public async Task<Result> Handle(RequestPurchaseOrderReceiptDateChangeCommand command, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = _validator.Validate(command);
        if (!validationResult.IsValid)
        {
            // Zwracamy pierwszy błąd walidacji dla uproszczenia
            var firstError = validationResult.Errors.First();
            return Result.Failure(new Error(firstError.PropertyName, firstError.ErrorMessage));
        }

        try
        {
            // Pobieramy ID użytkownika i fabryki z serwisu kontekstowego
            // To lepsze niż przesyłanie tych danych w każdej komendzie.
            var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
            var factoryId = _currentUserService.FactoryId ?? throw new BusinessRuleValidationException("Brak kontekstu fabryki dla użytkownika.");

            // Przetwarzamy każdą linię z komendy
            foreach (var line in command.OrderLines)
            {
                // Używamy metody fabrycznej z naszego agregatu, aby stworzyć nowy, spójny obiekt
                var changeRequest = PurchaseOrderReceiptDateChangeRequest.Create(
                    purchaseOrder: line.PurchaseOrder,
                    lineNumber: line.LineNumber,
                    sequence: line.Sequence,
                    newReceiptDate: line.NewReceiptDate,
                    dateType: command.DateType,
                    factoryId: factoryId,
                    userId: userId);

                // Dodajemy nowy agregat do repozytorium (na razie tylko w pamięci, w Change Trackerze)
                _requestRepository.Add(changeRequest);
            }

            // Zapisujemy wszystkie zmiany do bazy danych w jednej, atomowej transakcji
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Zwracamy wynik sukcesu
            return Result.Success();
        }
        catch (BusinessRuleValidationException ex)
        {
            // Przechwytujemy wyjątek domenowy i zwracamy go jako elegancki błąd
            return Result.Failure(new Error("Validation.Error", ex.Message));
        }
    }
}
