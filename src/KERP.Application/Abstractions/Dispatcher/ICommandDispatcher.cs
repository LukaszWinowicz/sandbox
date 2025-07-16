using KERP.Application.Abstractions.CQRS;

namespace KERP.Application.Abstractions.Dispatcher;

public interface ICommandDispatcher
{
    //Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
    //    where TCommand : class, ICommand;

    Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);

}
