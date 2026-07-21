namespace TaskManager.Domain.Common.Results;

public readonly record struct Success;

public readonly record struct Deleted;

public static class Result
{
    public static Success Success => default;

    public static Deleted Deleted => default;
}

public sealed class Result<TValue>
{
    private readonly TValue? _value;
    private readonly List<Error>? _errors;

    public bool IsSuccess { get; }

    public bool IsError => !IsSuccess;

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Cannot access the value of a failed result.");

    public List<Error> Errors => IsError ? _errors! : [];

    private Result(TValue value)
    {
        _value = value ?? throw new ArgumentNullException(nameof(value));
        IsSuccess = true;
    }

    private Result(List<Error> errors)
    {
        if (errors is null || errors.Count == 0)
        {
            throw new ArgumentException("Provide at least one error.", nameof(errors));
        }

        _errors = errors;
        IsSuccess = false;
    }

    public static implicit operator Result<TValue>(TValue value) => new(value);

    public static implicit operator Result<TValue>(Error error) => new([error]);

    public static implicit operator Result<TValue>(List<Error> errors) => new(errors);

    public TResult Match<TResult>(Func<TValue, TResult> onSuccess, Func<List<Error>, TResult> onError)
        => IsSuccess ? onSuccess(Value) : onError(Errors);
}
