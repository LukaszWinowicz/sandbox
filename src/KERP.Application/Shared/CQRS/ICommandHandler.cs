namespace KERP.Application.Shared.CQRS;

public interface ICommandHandler<TCommand>
{
    Task Handle(TCommand command, CancellationToken cancellationToken);
}
