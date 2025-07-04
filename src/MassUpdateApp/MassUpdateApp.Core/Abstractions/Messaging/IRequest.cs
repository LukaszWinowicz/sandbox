namespace MassUpdateApp.Core.Abstractions.Messaging;

/// <summary>
/// Represents a request (Command or Query) that returns a response.
/// </summary>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public interface IRequest<TResponse> { }
