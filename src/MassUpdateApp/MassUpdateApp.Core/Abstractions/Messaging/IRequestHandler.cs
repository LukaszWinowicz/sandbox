namespace MassUpdateApp.Core.Abstractions.Messaging;

/// <summary>
/// Defines a handler for a specific request type.
/// </summary>
/// <typeparam name="TRequest">The type of request to handle.</typeparam>
/// <typeparam name="TResponse">The type of response from the handler.</typeparam>
public interface IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Handles the given request.
    /// </summary>
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}
