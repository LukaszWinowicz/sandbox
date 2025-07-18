﻿using KERP.Domain.Abstractions;
using KERP.Domain.Abstractions.Results;
using KERP.Domain.Enums;

namespace KERP.Domain.Entities.MassUpdate.PurchaseOrder;

/// <summary>
/// Reprezentuje pojedyncze żądanie aktualizacji daty odbioru dla linii zamówienia zakupu.
/// Jest to Agregat w naszym systemie, śledzący historię i stan żądań.
/// </summary>
public class PurchaseOrderReceiptDateUpdateRequestEntity : AggregateRoot<Guid>
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

    // Prywatny konstruktor dla EF Core i metody fabrycznej.
    // Wywołuje konstruktor bazowy z nowym Guid.
    private PurchaseOrderReceiptDateUpdateRequestEntity() : base(Guid.NewGuid()) { }


    /// <summary>
    /// Metoda fabryczna do tworzenia nowego, poprawnego obiektu żądania aktualizacji.
    /// Gwarantuje, że obiekt jest zawsze tworzony w spójnym stanie.
    /// </summary>
    public static PurchaseOrderReceiptDateUpdateRequestEntity Create(
        string purchaseOrder,
        int lineNumber,
        int sequence,
        DateTime newReceiptDate,
        ReceiptDateUpdateType dateType,
        int factoryId,
        string userId)
    {
        return new PurchaseOrderReceiptDateUpdateRequestEntity
        {
            Id = Guid.NewGuid(),
            PurchaseOrder = purchaseOrder,
            LineNumber = lineNumber,
            Sequence = sequence,
            NewReceiptDate = newReceiptDate,
            DateType = dateType,
            FactoryId = factoryId,
            UserId = userId,
            CreatedAtUtc = DateTime.UtcNow,
            IsProcessed = false
        };

        // Przykład dodania zdarzenia domenowego - odkomentuj, gdy będzie potrzebne.
        // updateRequest.AddDomainEvent(new ReceiptUpdateRequestedEvent(updateRequest.Id, updateRequest.PurchaseOrder));
    }

    /// <summary>
    /// Metoda biznesowa do oznaczania żądania jako przetworzone.
    /// Enkapsuluje logikę zmiany stanu obiektu.
    /// </summary>
    public void MarkAsProcessed()
    {
        if (IsProcessed)
        {
            // Rzucenie wyjątku jest lepsze, bo informuje o błędnym użyciu logiki.
            throw new InvalidOperationException("To żądanie zostało już przetworzone.");
        }

        IsProcessed = true;
        ProcessedDate = DateTime.UtcNow;

        // Przykład dodania kolejnego zdarzenia domenowego.
        // AddDomainEvent(new ReceiptUpdateProcessedEvent(this.Id));
    }
}
