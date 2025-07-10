using Microsoft.Extensions.DependencyInjection;

namespace KERP.Application.Shared.Mediator;

public class DiyMediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    public DiyMediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task SendAsync(object command, CancellationToken cancellationToken = default)
    {
        // Pobieramy typ polecenia, które otrzymaliśmy
        var commandType = command.GetType();

        // Tworzymy generyczny typ handlera, np. ICommandHandler<PurchaseOrderReceiptDateUpdateCommand>
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(commandType);

        // Prosimy kontener DI o dostarczenie nam implementacji tego handlera
        var handler = _serviceProvider.GetRequiredService(handlerType);

        // Wywołujemy metodę Handle na znalezionym handlerze, przekazując mu polecenie
        // Używamy refleksji, aby wywołać metodę, nie znając jej dokładnego typu w czasie kompilacji
        var method = handler.GetType().GetMethod("Handle");
        if (method != null)
        {
            await (Task)method.Invoke(handler, new[] { command, cancellationToken });
        }
        else
        {
            throw new InvalidOperationException($"Handler for command '{commandType.Name}' does not have a 'Handle' method.");
        }
    }
}
