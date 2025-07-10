using KERP.Domain.Interfaces.Shared;

namespace KERP.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly KerpDbContext _context;

    public UnitOfWork(KerpDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
