using KERP.Application.Abstractions.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace KERP.Infrastructure.Messaging;

/// <summary>
/// A custom implementation of the Mediator pattern that support pipeline behaviors
/// to handle cross-cutting concerns.
/// </summary>
public class DiyMediator
{
    public readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="DiyMediator"/> class.
    /// </summary>
    /// <param name="serviceProvider">The application's service provider to resolve handlers.</param>
    public DiyMediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Sends a request through the pipeline to its corresponding handler.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <param name="request">The request to send.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation, containing the handler's response.</returns>
    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        // 1. Get the concrete type of the request (e.g., PurchaseOrderReceiptDateUpdateCommand)
        var requestType = request.GetType();

        // 2. Build the generic type of the handler we are looking for
        // e.g., IRequestHandler<PurchaseOrderReceiptDateUpdateCommand, List<string>>
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));

        // 3. Ask the DI container to provide an instance of that handler
        var handler = _serviceProvider.GetRequiredService(handlerType);

        // 4. Resolve all registered pipeline behaviors for the current request/response
        var pipelineType = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, typeof(TResponse));
        var behaviors = _serviceProvider.GetServices(pipelineType)
            .Cast<object>() // Cast to object to work with a common type
            .ToList();

        // 5. Create the delegate for the actual handler invocation
        Func<Task<TResponse>> handle = () => (Task<TResponse>)handler.GetType()
            .GetMethod("Handle")!
            .Invoke(handler, new object[] { request, cancellationToken })!;

        // 6. Chain the behaviors together, with the handler at the very end
        foreach (var behavior in behaviors)
        {
            var previousHandle = handle; // Capture the current state of the delegate
            handle = () => (Task<TResponse>)behavior.GetType()
                .GetMethod("Handle")!
                .Invoke(behavior, new object[] { request, previousHandle, cancellationToken })!;
        }

        // 7. Invoke the final, wrapped delegate
        return handle();
    }
}
