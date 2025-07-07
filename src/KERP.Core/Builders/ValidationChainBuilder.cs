using KERP.Core.Abstractions.Messaging;
using KERP.Core.Validators.Components;
using KERP.Core.Validators.Handlers;

namespace KERP.Core.Builders;

/// <summary>
/// A builder class that provides a fluent API for constructing
/// complex validation chains (Chain of Responsibility).
/// </summary>
public class ValidationChainBuilder
{
    private IValidationHandler? _head;
    private IValidationHandler? _tail;

    // Prywatna metoda pomocnicza do dodawania nowego ogniwa na końcu łańcucha
    private void AddHandler(IValidationHandler handler)
    {
        if (_head == null)
        {
            _head = handler;
            _tail = handler;
        }
        else
        {
            _tail!.SetNext(handler);
            _tail = handler;
        }
    }

    // --- Metody Budujące dla Każdego z Naszych "Klocków" ---

    public ValidationChainBuilder WithNotEmptyCheck(Func<CommandBase, string?> valueProvider, string fieldName)
    {
        AddHandler(new NotEmptyValidator(valueProvider, fieldName));
        return this;
    }

    public ValidationChainBuilder WithStringLengthCheck(Func<CommandBase, string?> valueProvider, int length, string fieldName)
    {
        AddHandler(new StringLengthValidator(valueProvider, length, fieldName));
        return this;
    }

    public ValidationChainBuilder WithNotNullCheck<T>(Func<CommandBase, T?> valueProvider, string fieldName)
    {
        AddHandler(new NotNullValidator<T>(valueProvider, fieldName));
        return this;
    }

    public ValidationChainBuilder WithFutureDateCheck(Func<CommandBase, DateTime?> dateProvider, string fieldName)
    {
        AddHandler(new FutureDateValidator(dateProvider, fieldName));
        return this;
    }

    public ValidationChainBuilder WithMinValueCheck<T>(Func<CommandBase, T> valueProvider, T minValue, string fieldName) where T : IComparable<T>
    {
        AddHandler(new MinValueValidator<T>(valueProvider, minValue, fieldName));
        return this;
    }

    public ValidationChainBuilder WithExistenceCheck<T>(Func<CommandBase, T> valueProvider, Func<T, Task<bool>> existenceCheckFunc, string fieldName)
    {
        AddHandler(new ExistenceValidator<T>(valueProvider, existenceCheckFunc, fieldName));
        return this;
    }

    public ValidationChainBuilder WithCombinationCheck<TCommand, TRepository>(TRepository repository, Func<TCommand, TRepository, Task<bool>> checkFunc, string errorMessage)
        where TCommand : CommandBase
    {
        AddHandler(new CombinationValidator<TCommand, TRepository>(repository, checkFunc, errorMessage));
        return this;
    }

    /// <summary>
    /// Finalizes the chain construction and returns the first handler.
    /// </summary>
    /// <returns>The first handler in the constructed chain.</returns>
    public IValidationHandler Build()
    {
        if (_head == null)
        {
            throw new InvalidOperationException("Cannot build an empty validation chain.");
        }
        return _head;
    }
}
