using KERP.Application.Abstractions.Messaging;
using KERP.Application.Interfaces.ValidationStrategies;
using KERP.Domain.Results;

namespace KERP.Application.Validation;

/// <summary>
/// A pipeline behavior that performs validation on a request before passing it to the handler.
/// It dynamically resolves validation strategies only for requests that are commands.
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
        // Sprawdzamy, czy request jest komendą, dla której mogą istnieć strategie walidacji.
        // Jeśli nie, po prostu idziemy dalej.
        if (request is not CommandBase)
        {
            return await next();
        }

        // Tworzymy typ generyczny strategii walidacji dla konkretnego typu TRequest
        var strategyType = typeof(IValidationStrategy<>).MakeGenericType(request.GetType());

        // Używamy ServiceProvider, aby spróbować pobrać strategię.
        // GetService zwróci null, jeśli strategia nie jest zarejestrowana, co jest oczekiwane.
        var validationStrategy = _serviceProvider.GetService(strategyType);

        // Jeśli dla tej komendy nie ma zarejestrowanej strategii, idziemy dalej.
        if (validationStrategy is null)
        {
            return await next();
        }

        // Dynamicznie wywołujemy metodę ValidateAsync na znalezionej strategii.
        var validationResult = (Task<List<RowValidationResult>>)validationStrategy.GetType()
            .GetMethod("ValidateAsync")!
            .Invoke(validationStrategy, new object[] { request });

        var validationErrors = await validationResult;

        if (validationErrors.Any(r => !r.IsValid))
        {
            // Błędy walidacji istnieją. Musimy je zwrócić w odpowiednim formacie.
            // Zakładając, że TResponse to `List<RowValidationResult>`.
            if (typeof(TResponse) == typeof(List<RowValidationResult>))
            {
                return validationErrors as TResponse;
            }

            // Jeśli typ odpowiedzi jest inny, musimy zdecydować, co zrobić.
            // Rzucenie wyjątku jest bezpiecznym domyślnym zachowaniem.
            throw new InvalidOperationException(
                "Validation failed, but the response type cannot be cast from List<RowValidationResult>."
            );
        }

        // Walidacja pomyślna, przechodzimy do następnego kroku w pipeline.
        return await next();
    }
}