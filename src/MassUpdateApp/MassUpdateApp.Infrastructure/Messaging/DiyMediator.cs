using MassUpdateApp.Core.Abstractions.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace MassUpdateApp.Infrastructure.Messaging;

/// <summary>
/// A simple, custom implementation of the Mediator pattern.
/// </summary>
public class DiyMediator
{
    private readonly IServiceProvider _serviceProvider;

    public DiyMediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        // 1. Pobierz typ konkretnego polecenia (np. ProcessUpdateReceiptDateCommand)
        var requestType = request.GetType();

        // 2. Zbuduj typ generycznego handlera, którego szukamy
        // np. IRequestHandler<ProcessUpdateReceiptDateCommand, List<string>>
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));

        // 3. Poproś kontener DI o podanie instancji tego handlera
        var handler = _serviceProvider.GetRequiredService(handlerType);

        // 4. Wywołaj metodę "Handle" na znalezionym handlerze, przekazując polecenie
        return (Task<TResponse>)handler.GetType()
              .GetMethod("Handle")!
              .Invoke(handler, new object[] { request, cancellationToken })!;
    }
}
