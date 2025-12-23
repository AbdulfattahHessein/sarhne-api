using System.Diagnostics.CodeAnalysis;

namespace Core;

public class Result<TValue>
{
    public readonly TValue? Value;
    public readonly Error? Error;
    private readonly bool _succeeded;

    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Succeeded => _succeeded;

    [MemberNotNullWhen(true, nameof(Error))]
    [MemberNotNullWhen(false, nameof(Value))]
    public bool Failed => !_succeeded;

    private Result(TValue value)
    {
        Value = value;

        _succeeded = true;
    }

    private Result(Error error)
    {
        Error = error;

        _succeeded = false;
    }

    //happy path
    public static implicit operator Result<TValue>(TValue value) => new(value);

    public static implicit operator TValue?(Result<TValue> result) => result.Value;

    //error path
    public static implicit operator Result<TValue>(Error error) => new(error);

    public static implicit operator Error?(Result<TValue> result) => result.Error;

    public static Result<TValue> Success(TValue value) => new(value);

    public static Result<TValue> Failure(Error error) => new(error);

    public TResult Match<TResult>(Func<TValue, TResult> onSuccess, Func<Error, TResult> onFailure)
    {
        return Succeeded ? onSuccess(Value) : onFailure(Error);
    }
}
