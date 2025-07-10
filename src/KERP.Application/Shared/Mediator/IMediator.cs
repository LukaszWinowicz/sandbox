namespace KERP.Application.Shared.Mediator;

public interface IMediator
{
    Task SendAsync(object command, CancellationToken cancellationToken = default);
}
