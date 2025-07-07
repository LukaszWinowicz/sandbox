using KERP.Core.Interfaces.Repositories;
namespace KERP.Infrastructure.Persistence.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly AppDbContext _context;

    public Repository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        // context.Set<TEntity>() to metoda EF Core, która dynamicznie
        // zwraca odpowiedni DbSet dla danego typu encji (np. PurchaseOrderReceiptDateUpdateEntity).
        await _context.Set<TEntity>().AddRangeAsync(entities);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
