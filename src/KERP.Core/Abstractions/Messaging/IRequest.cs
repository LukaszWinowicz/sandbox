namespace KERP.Core.Abstractions.Messaging;

/// <summary>
/// Represents a request (e.g., a Command or Query) that returns a response.
/// </summary>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public interface IRequest<TResponse> { }