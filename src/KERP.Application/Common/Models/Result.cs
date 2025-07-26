namespace KERP.Application.Common.Models;

/// <summary>
/// Reprezentuje wynik operacji bez zwracanej wartości.
/// </summary>
public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public IReadOnlyCollection<Error> Errors { get; }

    protected Result(bool isSuccess, IReadOnlyCollection<Error> errors)
    {
        IsSuccess = isSuccess;
        Errors = errors ?? new List<Error>();
    }
    /// <summary>
    /// Tworzy wynik operacji zakończonej sukcesem.
    /// </summary>
    public static Result Success(List<RowValidationResult> results) => new Result(true, Array.Empty<Error>());

    /// <summary>
    /// Tworzy wynik operacji zakończonej niepowodzeniem.
    /// </summary>
    public static Result Failure(IReadOnlyCollection<Error> errors) => new Result(false, errors);
}

/// <summary>
/// Reprezentuje wynik operacji z zwracaną wartością.
/// </summary>
/// <typeparam name="TValue"></typeparam>
public class Result<TValue> : Result
{
    private readonly TValue _value;
    public TValue Value => IsSuccess ? _value : throw new InvalidOperationException("Nie można uzyskać wartości z rezultatu błędu.");

    public Result(TValue value)
        : base(true, Array.Empty<Error>())
    {
        _value = value;
    }

    protected Result(IReadOnlyCollection<Error> errors) : base(false, errors)
    {
    }
    /// <summary>
    /// Tworzy wynik operacji zakończonej sukcesem z wartością.
    /// </summary>
    public static Result<TValue> Success(TValue value) => new Result<TValue>(value);
    /// <summary>
    /// Tworzy rezulatat błędu z kolacką błędów.
    /// </summary>
    public static Result<TValue> Failure(IReadOnlyCollection<Error> errors) => new Result<TValue>(errors);
}