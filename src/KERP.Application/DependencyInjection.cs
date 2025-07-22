using KERP.Application.Common.Abstractions;
using KERP.Application.Common.Dispatchers;
using KERP.Application.Common.Models;
using KERP.Application.Features.MassUpdates.PurchaseOrder.GetChangeRequests;
using KERP.Application.Features.MassUpdates.PurchaseOrder.UpdateReceiptDate;
using KERP.Application.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace KERP.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Rejestracja Dispatcherów
        services.AddScoped<ICommandDispatcher, CommandDispatcher>();
        services.AddScoped<IQueryDispatcher, QueryDispatcher>();

        // Ręczna rejestracja naszego pierwszego handlera.
        // W dużej aplikacji użylibyśmy refleksji, aby automatycznie znaleźć
        // i zarejestrować wszystkie handlery, ale na razie to wystarczy.
        services.AddScoped<ICommandHandler<RequestPurchaseOrderReceiptDateChangeCommand, Result>,
            RequestPurchaseOrderReceiptDateChangeCommandHandler>();

        // Tutaj w przyszłości zarejestrujemy też walidatory i inne serwisy aplikacyjne.
        services.AddScoped<IValidator<RequestPurchaseOrderReceiptDateChangeCommand>,
        RequestPurchaseOrderReceiptDateChangeCommandValidator>();

        services.AddScoped<IQueryHandler<GetChangeRequestsQuery, List<ChangeRequestDto>>,
        GetChangeRequestsQueryHandler>();

        return services;
    }
}
