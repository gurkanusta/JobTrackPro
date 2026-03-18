namespace JobTrackPro.Application.Common;

public class Result
{
    protected Result(bool isSuccess, string? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public string? Error { get; }
    public bool IsFailure => !IsSuccess;

    public static Result Success() => new(true, null);
    public static Result Failure(string error) => new(false, error);
}

public sealed class Result<T> : Result
{
    private Result(T value) : base(true, null) => Value = value;
    private Result(string error) : base(false, error) { }

    public T? Value { get; }

    public static Result<T> Success(T value) => new(value);
    public new static Result<T> Failure(string error) => new(error);
}