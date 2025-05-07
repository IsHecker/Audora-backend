namespace Audora.Application.Common.Results;

public class Result
{
    private readonly List<Error>? _errors;

    public bool IsSuccess => !IsError;
    public bool IsError { get; }

    public List<Error> Errors => IsError ? _errors! : [Error.NoErrors];

    protected Result()
    {
        _errors = null;
        IsError = false;
    }

    protected Result(Error error)
    {
        _errors = [error];
        IsError = true;
    }

    protected Result(IEnumerable<Error> errors)
    {
        _errors = errors?.ToList() ?? throw new ArgumentNullException(nameof(errors));
        IsError = true;
    }

    public static Result Success => new();

    public static Result Failure(params Error[] errors) => new(errors);
    public static Result Failure(IEnumerable<Error> errors) => new(errors);

    public static implicit operator Result(Error error) => new(error);
    public static implicit operator Result(List<Error> errors) => new(errors);

    public TResult MatchFirst<TResult>(Func<TResult> onSuccess, Func<Error, TResult> onFailure)
    {
        return IsSuccess ? onSuccess() : onFailure(Errors[0]);
    }

    public TResult Match<TResult>(Func<TResult> onSuccess, Func<IReadOnlyList<Error>, TResult> onFailure)
    {
        return IsSuccess ? onSuccess() : onFailure(_errors!);
    }

    public void Match(Action onSuccess, Action<IReadOnlyList<Error>> onFailure)
    {
        if (IsSuccess)
            onSuccess();
        else
            onFailure(_errors!);
    }
}

public class Result<TValue> : Result
{
    private readonly TValue _value;

    public TValue Value =>
        IsSuccess ? _value : throw new InvalidOperationException("Cannot access Value when Result is error.");

    public Result(TValue value)
    {
        _value = value;
    }

    private Result(IEnumerable<Error> errors) : base(errors)
    {
        _value = default!;
    }

    public new static Result<TValue> Failure(params Error[] errors) => new(errors);
    public new static Result<TValue> Failure(IEnumerable<Error> errors) => new(errors);

    public static implicit operator Result<TValue>(TValue value) => new(value);
    public static implicit operator Result<TValue>(Error error) => Failure(error);
    public static implicit operator Result<TValue>(List<Error> errors) => Failure(errors);

    public TResult MatchFirst<TResult>(Func<TValue, TResult> onSuccess, Func<Error, TResult> onFailure)
    {
        return IsSuccess ? onSuccess(_value) : onFailure(Errors[0]);
    }

    public TResult Match<TResult>(Func<TValue, TResult> onSuccess, Func<List<Error>, TResult> onFailure)
    {
        return IsSuccess ? onSuccess(_value) : onFailure(Errors);
    }

    public void Match(Action<TValue> onSuccess, Action<List<Error>> onFailure)
    {
        if (IsSuccess)
            onSuccess(_value);
        else
            onFailure(Errors);
    }
}

public static class ResultExtensions
{
    public static Result<T> ToResult<T>(this T value)
    {
        return new Result<T>(value);
    }
}