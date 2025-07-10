using KERP.Domain.Entities.Shared;
using KERP.Domain.Interfaces.Shared;
using Microsoft.EntityFrameworkCore;

namespace KERP.Infrastructure.Persistence.Repositories;

public class FactoryRepository : IFactoryRepository
{
    private readonly KerpDbContext _context;

    public FactoryRepository(KerpDbContext context)
    {
        _context = context;
    }

    public async Task<FactoryEntity?> GetByIdAsync(int factoryId, CancellationToken cancellationToken = default)
    {
        return await _context.Factories
            .AsNoTracking() // Używamy AsNoTracking, bo są to dane tylko do odczytu
            .FirstOrDefaultAsync(f => f.Id == factoryId, cancellationToken);
    }
}
