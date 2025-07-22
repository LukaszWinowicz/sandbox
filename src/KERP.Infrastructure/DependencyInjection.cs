using KERP.Application.Common.Abstractions;
using KERP.Application.Services;
using KERP.Domain.Aggregates.PurchaseOrder;
using KERP.Infrastructure.Persistence;
using KERP.Infrastructure.Persistence.Configurations;
using KERP.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KERP.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Rejestracja DbContext
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Rejestracja naszych implementacji jako Scoped
        // Oznacza to, że jeden obiekt będzie używany w ramach jednego żądania HTTP.
        services.AddScoped<IPurchaseOrderReceiptDateChangeRequestRepository, PurchaseOrderReceiptDateChangeRequestRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}
