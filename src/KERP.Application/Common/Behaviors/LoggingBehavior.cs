using KERP.Application.Common.Abstractions;
using Microsoft.Extensions.Logging;

namespace KERP.Application.Common.Behaviors;

// Używamy generyków, aby ten behavior mógł obsłużyć dowolną komendę
public class LoggingBehavior<TCommand, TResult> : ICommandHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    private readonly ICommandHandler<TCommand, TResult> _decorated;
    private readonly ILogger<LoggingBehavior<TCommand, TResult>> _logger;

    public LoggingBehavior(
        ICommandHandler<TCommand, TResult> decorated,
        ILogger<LoggingBehavior<TCommand, TResult>> logger)
    {
        _decorated = decorated;
        _logger = logger;
    }

    public async Task<TResult> Handle(TCommand command, CancellationToken cancellationToken)
    {
        var commandName = typeof(TCommand).Name;

        _logger.LogInformation("Rozpoczynanie obsługi komendy: {CommandName}", commandName);

        var result = await _decorated.Handle(command, cancellationToken);

        _logger.LogInformation("Zakończono obsługę komendy: {CommandName}", commandName);

        return result;
    }
}
