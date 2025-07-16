using KERP.Application.Abstractions.CQRS;
using Microsoft.Extensions.DependencyInjection;

namespace KERP.Application.Abstractions.Dispatcher;
public sealed class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public CommandDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
    {
        // Znajdź typ handlera, który pasuje do ICommandHandler<TCommand, TResult>
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));

        // Pobierz usługę handlera z kontenera DI
        dynamic handler = _serviceProvider.GetRequiredService(handlerType);

        // Wywołaj metodę HandleAsync na znalezionym handlerze
        return await handler.HandleAsync((dynamic)command, cancellationToken);
    }
}
