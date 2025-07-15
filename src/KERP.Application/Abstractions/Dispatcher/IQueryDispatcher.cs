using KERP.Application.Abstractions.CQRS;

namespace KERP.Application.Abstractions.Dispatcher;

public interface IQueryDispatcher
{
    Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
}
