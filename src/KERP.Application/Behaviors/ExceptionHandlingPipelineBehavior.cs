using KERP.Application.Abstractions.Messaging;
using KERP.Application.Interfaces.ValidationStrategies;
using KERP.Domain.Results;
using System.Reflection;

namespace KERP.Application.Behaviors;

/// <summary>
/// A pipeline behavior that catches exceptions and converts them into a failure Result object.
/// This ensures that handlers don't leak exceptions and the application layer returns consistent response types.
/// </summary>
public class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : class
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationPipelineBehavior(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> Handle(TRequest request, Func<Task<TResponse>> next, CancellationToken cancellationToken)
    {
        if (request is not CommandBase)
        {
            return await next();
        }

        var strategyType = typeof(IValidationStrategy<>).MakeGenericType(request.GetType());
        var validationStrategy = _serviceProvider.GetService(strategyType);

        if (validationStrategy is null)
        {
            return await next();
        }

        var validationResultTask = (Task<List<RowValidationResult>>)validationStrategy.GetType()
            .GetMethod("ValidateAsync")!
            .Invoke(validationStrategy, new object[] { request });

        var validationErrors = await validationResultTask;

        if (validationErrors.Any(r => !r.IsValid))
        {
            // Błędy istnieją. Tworzymy obiekt Result.Failure.
            // Używamy refleksji, tak jak w ExceptionHandlingPipelineBehavior.

            // 1. Sprawdzamy, czy TResponse to Result<T>
            var responseType = typeof(TResponse);
            if (!responseType.IsGenericType || responseType.GetGenericTypeDefinition() != typeof(Result<>))
            {
                throw new InvalidOperationException("Validation failed, but response type is not a Result<T>.");
            }

            // 2. Tworzymy błąd walidacyjny.
            // Możesz tu zbudować bardziej szczegółowy komunikat.
            var error = new Error("Validation.Failed", "One or more validation errors occurred.");

            // 3. Pobieramy typ generyczny z Result<T>
            var resultType = responseType.GetGenericArguments()[0];

            // 4. Tworzymy obiekt Result<T>.Failure(error)
            var genericResultType = typeof(Result<>).MakeGenericType(resultType);
            var failureMethod = genericResultType.GetMethod("Failure", BindingFlags.Public | BindingFlags.Static);
            var failureResult = failureMethod.Invoke(null, new object[] { error });

            // Możesz też chcieć przekazać `validationErrors` wewnątrz Result.
            // Na razie zwracamy ogólny błąd.

            return (TResponse)failureResult;
        }

        return await next();
    }
}