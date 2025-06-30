using MassUpdate.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace MassUpdate.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // W tym miejscu w przyszłości zdefiniujemy nasze tabele w bazie danych, np.:
        public DbSet<PurchaseOrderUpdateRequestEntity> PurchaseOrderUpdateRequests { get; set; }

    }
}
