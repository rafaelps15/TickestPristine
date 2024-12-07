
//namespace Tickest.Domain.Common;

//public class Result<T>
//{
//    public bool IsSuccess { get; set; }
//    public T Data { get; set; }
//    public string ErrorMessage { get; set; }

//    public static Result<T> Success(T data) => new Result<T> { IsSuccess = true, Data = data };
//    public static Result<T> Failure(string errorMessage) => new Result<T> { IsSuccess = false, ErrorMessage = errorMessage };

//}

using System;
using System.Diagnostics.CodeAnalysis;
using Tickest.Domain.Exceptions;

namespace Tickest.Domain.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; }

        protected Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None || !isSuccess && error == Error.None)
            {
                throw new TickestException("Estado de erro inválido.", nameof(error));
            }

            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new Result(true, Error.None);

        public static Result Failure(Error error) => new Result(false, error);

        public static Result<TValue> Success<TValue>(TValue value) =>
            new Result<TValue>(value, true, Error.None);

        public static Result<TValue> Failure<TValue>(Error error) =>
            new Result<TValue>(default, false, error);
    }

    public class Result<TValue> : Result
    {
        private readonly TValue? _value;

        internal Result(TValue? value, bool isSuccess, Error error)
            : base(isSuccess, error)
        {
            _value = value;
        }

        [NotNull]
        public TValue Value => IsSuccess
            ? _value!
            : throw new TickestException("O valor de um resultado com falha não pode ser acessado.");

        public static implicit operator Result<TValue>(TValue? value) =>
            value != null ? Success(value) : Failure<TValue>(Error.NullValue);

        public static Result<TValue> ValidationFailure(Error error) =>
            new Result<TValue>(default, false, error);
    }

    public class Error
    {
        public static readonly Error None = new(string.Empty);
        public static readonly Error NullValue = new("O valor não pode ser nulo.");

        public string Message { get; }

        private Error(string message)
        {
            Message = message;
        }

        public override string ToString() => Message;
    }
}
