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

    public async Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : class, ICommand
    {
        var handlerType = typeof(ICommandHandler<TCommand>);
        dynamic handler = _serviceProvider.GetRequiredService(handlerType);
        await handler.HandleAsync(command, cancellationToken);
    }
}
