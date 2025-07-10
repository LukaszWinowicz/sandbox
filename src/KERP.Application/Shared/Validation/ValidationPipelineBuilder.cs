namespace KERP.Application.Shared.Validation;

/// <summary>
/// Generyczny budowniczy do tworzenia łańcucha walidacyjnego.
/// </summary>
public class ValidationPipelineBuilder<T> where T : class
{
    private IValidationHandler<T>? _head;
    private IValidationHandler<T>? _current;

    public ValidationPipelineBuilder<T> Add(IValidationHandler<T> handler)
    {
        if (_head == null)
        {
            _head = handler;
            _current = handler;
        }
        else
        {
            _current?.SetNext(handler);
            _current = handler;
        }
        return this;
    }

    public IValidationHandler<T> Build()
    {
        if (_head == null) throw new InvalidOperationException("Cannot build an empty validation pipeline.");
        return _head;
    }
}