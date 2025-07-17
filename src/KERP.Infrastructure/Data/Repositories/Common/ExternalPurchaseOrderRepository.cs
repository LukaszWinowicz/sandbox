using KERP.Application.Abstractions.Repositories.Common;
using KERP.Application.Common.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace KERP.Infrastructure.Data.Repositories.Common;

public class ExternalPurchaseOrderRepository : IExternalPurchaseOrderRepository
{
    private readonly AppDbContext _context;
    private readonly ICurrentUserContext _currentUserContext;
    public ExternalPurchaseOrderRepository(AppDbContext context, ICurrentUserContext currentUserContext)
    {
        _context = context;
        _currentUserContext = currentUserContext;
    }

    public async Task<bool> ExistsAsync(string purchaseOrder, CancellationToken cancellationToken = default)
    {
        int factoryId = _currentUserContext.FactoryId;
        // Krok 1: Zbuduj dynamicznie nazwę tabeli zgodnie z konwencją
        string tableName = $"bgq.PurchaseOrder_{factoryId}";

        // Krok 2: Przygotuj bezpieczne zapytanie SQL.
        // Używamy interpolacji stringów do wstawienia nazwy tabeli (bezpieczne, bo factoryId to int).
        // Używamy parametryzacji EF Core dla danych wejściowych od użytkownika (@p0).
        //var sql = $"SELECT 1 FROM {tableName} WHERE PurchaseOrder = {{0}}";
        var sql = $"SELECT COUNT(*) FROM {tableName} WHERE PurchaseOrder = @po";
        var count = await _context.Database.ExecuteSqlRawAsync(sql, new[] {
    new SqlParameter("@po", purchaseOrder)
});
        return count > 0;

        // Krok 3: Wykonaj zapytanie używając FromSqlRaw na DbSet.
        // To jest potężny mechanizm EF Core do mieszania SQL z LINQ.
        // Pytamy tylko o istnienie, więc AnyAsync jest idealne i wydajne.
        var exists = await _context.ExternalPurchaseOrderLines
            .FromSqlRaw(sql, purchaseOrder)
            .AnyAsync(cancellationToken);

        return exists;
    }
}