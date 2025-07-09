namespace KERP.Application.Abstractions.Messaging;

/// <summary>
/// Defines a pipeline behavior for processing requests.
/// It allows for adding cross-cutting concerns around the inner request handler.
/// </summary>
/// <typeparam name="TRequest">The type of the request being handled.</typeparam>
/// <typeparam name="TResponse">The type of the response from the handler.</typeparam>
public interface IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Handles the request and executes the next action in the pipeline.
    /// </summary>
    /// <param name="request">The request to process.</param>
    /// <param name="next">The delegate representing the next action in the pipeline.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation, containing the handler's response.</returns>
    Task<TResponse> Handle(TRequest request, Func<Task<TResponse>> next, CancellationToken cancellationToken);
}
