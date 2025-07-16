namespace KERP.Domain.Abstractions.Results;

/// <summary>
/// Reprezentuje wynik operacji, która może zakończyć się sukcesem lub porażką.
/// </summary>
/// <typeparam name="T">Typ wartości zwracanej w przypadku sukcesu.</typeparam>
public class Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T? Value { get; }
    public IReadOnlyCollection<string> Errors { get; }

    private Result(T value)
    {
        IsSuccess = true;
        Value = value;
        Errors = Array.Empty<string>();
    }

    private Result(IReadOnlyCollection<string> errors)
    {
        IsSuccess = false;
        Value = default;
        Errors = errors ?? new[] { "Wystąpił nieznany błąd." };
    }

    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failure(IReadOnlyCollection<string> errors) => new(errors);
    public static Result<T> Failure(string error) => new(new[] { error });
}