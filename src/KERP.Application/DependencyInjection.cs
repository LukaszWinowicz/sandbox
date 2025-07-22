using KERP.Application.Common.Abstractions;
using KERP.Application.Common.Behaviors;
using KERP.Application.Common.Dispatchers;
using KERP.Application.Common.Models;
using KERP.Application.Features.MassUpdates.PurchaseOrder.GetChangeRequests;
using KERP.Application.Features.MassUpdates.PurchaseOrder.UpdateReceiptDate;
using KERP.Application.Validation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KERP.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Rejestracja Dispatcherów
        services.AddScoped<ICommandDispatcher, CommandDispatcher>();
        services.AddScoped<IQueryDispatcher, QueryDispatcher>();

        // Musimy teraz ręcznie zarejestrować każdy walidator i handler.
        // Krok 1: Rejestrujemy konkretny walidator.
        services.AddScoped<IValidator<RequestPurchaseOrderReceiptDateChangeCommand>, RequestPurchaseOrderReceiptDateChangeCommandValidator>();

        // Krok 2: Rejestrujemy KONKRETNĄ klasę handlera.
        services.AddScoped<RequestPurchaseOrderReceiptDateChangeCommandHandler>();

        // Krok 3: Rejestrujemy INTERFEJS handlera, ręcznie budując łańcuch dekoratorów.
        services.AddScoped<ICommandHandler<RequestPurchaseOrderReceiptDateChangeCommand, Result>>(provider =>
        {
            // Pobieramy "prawdziwy" handler, który będzie na końcu łańcucha.
            var commandHandler = provider.GetRequiredService<RequestPurchaseOrderReceiptDateChangeCommandHandler>();

            // Tworzymy pierwszy dekorator (Validation), owijając nim prawdziwy handler.
            var validationBehavior = new ValidationBehavior<RequestPurchaseOrderReceiptDateChangeCommand, Result>(
                commandHandler,
                provider.GetRequiredService<IEnumerable<IValidator<RequestPurchaseOrderReceiptDateChangeCommand>>>());

            // Tworzymy drugi dekorator (Logging), owijając nim poprzedni dekorator (Validation).
            var loggingBehavior = new LoggingBehavior<RequestPurchaseOrderReceiptDateChangeCommand, Result>(
                validationBehavior,
                provider.GetRequiredService<ILogger<LoggingBehavior<RequestPurchaseOrderReceiptDateChangeCommand, Result>>>());

            // Zwracamy najbardziej zewnętrzny dekorator.
            return loggingBehavior;
        });

        // Query Handlery na razie rejestrujemy prosto, bo nie mają dekoratorów
        services.AddScoped<IQueryHandler<GetChangeRequestsQuery, List<ChangeRequestDto>>, GetChangeRequestsQueryHandler>();

        return services;
    }
}
