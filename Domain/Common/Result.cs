
//namespace Tickest.Domain.Common;

//public class Result<T>
//{
//    public bool IsSuccess { get; set; }
//    public T Data { get; set; }
//    public string ErrorMessage { get; set; }

//    public static Result<T> Success(T data) => new Result<T> { IsSuccess = true, Data = data };
//    public static Result<T> Failure(string errorMessage) => new Result<T> { IsSuccess = false, ErrorMessage = errorMessage };

//}

using System.Diagnostics.CodeAnalysis;

namespace Tickest.Domain.Common;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None || !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Estado de erro inválido.", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new Result(true, Error.None);

    public static Result Failure(Error error) => new Result(false, error);

    public static Result<T> Success<T>(T value) => new Result<T>(value, true, Error.None);

    public static Result<T> Failure<T>(Error error) => new Result<T>(default, false, error);
}

public class Result<T> : Result
{
    private readonly T? _value;

    internal Result(T? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    [NotNull]
    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("O valor de um resultado com falha não pode ser acessado.");

    public static implicit operator Result<T>(T value) =>
        value is not null ? Success(value) : Failure<T>(Error.NullValue);

    public static Result<T> ValidationFailure(Error error) =>
        new Result<T>(default, false, error);
}

public class Error
{
    public static readonly Error None = new Error(string.Empty);
    public static readonly Error NullValue = new Error("O valor não pode ser nulo.");

    public string Message { get; }

    private Error(string message)
    {
        Message = message;
    }

    public override string ToString() => Message;
}
