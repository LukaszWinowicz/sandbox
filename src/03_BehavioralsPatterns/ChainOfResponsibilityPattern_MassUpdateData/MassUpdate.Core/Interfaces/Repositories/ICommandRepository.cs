using MassUpdate.Core.Entities;

namespace MassUpdate.Core.Interfaces.Repositories;

public interface ICommandRepository
{
    /// <summary>
    /// Dodaje nową prośbę o aktualizację zlecenia zakupu do bazy danych.
    /// </summary>
    /// <param name="entity">W pełni zwalidowana i zmapowana encja.</param>
    Task AddPurchaseOrderUpdateRequestAsync(PurchaseOrderUpdateRequestEntity entity);

    // W przyszłości:
    // Task AddProductionOrderUpdateRequestAsync(ProductionOrderUpdateRequestEntity entity);
}
