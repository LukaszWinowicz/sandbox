using MassUpdate.Core.Entities;
using MassUpdate.Core.Interfaces.Repositories;
using System;

namespace MassUpdate.Infrastructure.Persistence.Repositories;

public class CommandRepository : ICommandRepository
{
    private readonly AppDbContext _context;

    public CommandRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddPurchaseOrderUpdateRequestAsync(PurchaseOrderUpdateRequestEntity entity)
    {
        // Dodajemy nową encję do kontekstu EF Core
        await _context.PurchaseOrderUpdateRequests.AddAsync(entity);

        // Zapisujemy wszystkie śledzone zmiany do bazy danych
        await _context.SaveChangesAsync();
    }
}
