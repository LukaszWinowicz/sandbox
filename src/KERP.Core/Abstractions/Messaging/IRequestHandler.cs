namespace KERP.Core.Abstractions.Messaging;

/// <summary>
/// Defines a handler for a specific request.
/// </summary>
/// <typeparam name="TRequest">The type of the request to handler.</typeparam>
/// <typeparam name="TResponse">the type of response from the handler.</typeparam>
public interface IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Handles the given request and returns a response.
    /// </summary>
    /// <param name="request">The request object.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the response.</returns>
    Task<TResponse> Handler(TRequest request, CancellationToken cancellationToken);
}
