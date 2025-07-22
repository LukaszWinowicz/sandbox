using KERP.Domain.Common;
using KERP.Domain.Events.PurchaseOrder;

namespace KERP.Domain.Aggregates.PurchaseOrder;

/// <summary>
/// Reprezentuje pojedyncze żądanie zmiany daty odbioru dla linii zamówienia zakupu.
/// Jest to Agregat śledzący historię i stan żądania.
/// </summary>
public class PurchaseOrderReceiptDateChangeRequest : AggregateRoot<int>
{
    public string PurchaseOrder { get; private set; }
    public int LineNumber { get; private set; }
    public int Sequence { get; private set; }
    public DateTime NewReceiptDate { get; private set; }
    public ReceiptDateUpdateType DateType { get; private set; }
    public int FactoryId { get; private set; }
    public string UserId { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public bool IsProcessed { get; private set; }
    public DateTime? ProcessedDate { get; private set; }

    // Prywatny, pusty konstruktor dla EF Core
    private PurchaseOrderReceiptDateChangeRequest() : base() { }

    // Prywatny konstruktor wywoływany przez metodę fabryczną.
    // Gwarantuje, że obiekt jest tworzony w spójnym stanie.
    private PurchaseOrderReceiptDateChangeRequest(
        string purchaseOrder,
        int lineNumber,
        int sequence,
        DateTime newReceiptDate,
        ReceiptDateUpdateType dateType,
        int factoryId,
        string userId) : base() // ID zostanie nadane przez bazę danych
    {
        PurchaseOrder = purchaseOrder;
        LineNumber = lineNumber;
        Sequence = sequence;
        NewReceiptDate = newReceiptDate;
        DateType = dateType;
        FactoryId = factoryId;
        UserId = userId;
        CreatedAtUtc = DateTime.UtcNow; // Dla uproszczenia, w realnym systemie użylibyśmy IClock
        IsProcessed = false;
    }

    /// <summary>
    /// Metoda fabryczna do tworzenia nowego, poprawnego obiektu żądania.
    /// </summary>
    public static PurchaseOrderReceiptDateChangeRequest Create(
        string purchaseOrder,
        int lineNumber,
        int sequence,
        DateTime newReceiptDate,
        ReceiptDateUpdateType dateType,
        int factoryId,
        string userId)
    {
        // Krok 1: Walidacja niezmienników (reguł biznesowych)
        /*if (string.IsNullOrWhiteSpace(purchaseOrder))
            throw new BusinessRuleValidationException("Numer zamówienia nie może być pusty.");

        if (lineNumber <= 0)
            throw new BusinessRuleValidationException("Numer linii musi być dodatni.");

        if (string.IsNullOrWhiteSpace(userId))
            throw new BusinessRuleValidationException("Identyfikator użytkownika jest wymagany.");*/

        // Krok 2: Utworzenie obiektu
        var request = new PurchaseOrderReceiptDateChangeRequest(
            purchaseOrder,
            lineNumber,
            sequence,
            newReceiptDate,
            dateType,
            factoryId,
            userId);

        // Krok 3: Dodanie zdarzenia domenowego
        request.AddDomainEvent(new PurchaseOrderReceiptDateChangeRequestedEvent(request.Id, request.PurchaseOrder));

        return request;
    }

    /// <summary>
    /// Oznacza żądanie jako przetworzone.
    /// </summary>
    public void MarkAsProcessed()
    {
        if (IsProcessed)
        {
            // Można rzucić wyjątek lub po prostu zignorować
            return;
        }

        IsProcessed = true;
        ProcessedDate = DateTime.UtcNow;
        // Opcjonalnie: dodaj kolejne zdarzenie, np. ReceiptUpdateProcessedEvent
    }
}
