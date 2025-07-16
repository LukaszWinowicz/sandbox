namespace KERP.Application.Abstractions.CQRS;

/// <summary>
/// Interfejs dla handlera obsługującego komendę.
/// Odpowiada za wykonanie logiki związanej z komendą.
/// </summary>
/// <typeparam name="TCommand">Typ komendy, który handler obsługuje.</typeparam>
public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
{
    /// <summary>
    /// Obsługuje daną komendę.
    /// </summary>
    /// <param name="command">Instancja komendy do obsłużenia.</param>
    /// <param name="cancellationToken">Token anulujący operację.</param>
    /// <returns>Task reprezentujący operację asynchroniczną.</returns>
    Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken);
}