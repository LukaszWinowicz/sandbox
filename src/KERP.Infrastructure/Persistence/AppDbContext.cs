using KERP.Domain.Aggregates.PurchaseOrder;
using KERP.Domain.Aggregates.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace KERP.Infrastructure.Persistence;

// Zmieniamy dziedziczenie z DbContext na IdentityDbContext<ApplicationUser>
public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<PurchaseOrderReceiptDateChangeRequest> PurchaseOrderReceiptDateChangeRequests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Ta linijka jest teraz jeszcze ważniejsza.
        // Najpierw wołamy base.OnModelCreating, aby ASP.NET Identity
        // mogło skonfigurować swoje własne tabele (Users, Roles, Claims itp.).
        base.OnModelCreating(modelBuilder);

        // Następnie stosujemy nasze własne, niestandardowe konfiguracje.
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}