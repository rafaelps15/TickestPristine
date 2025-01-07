using System.Diagnostics.CodeAnalysis;
using Tickest.Domain.Exceptions;

namespace Tickest.Domain.Common
{
    public class Result
    {
        protected Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None || !isSuccess && error == Error.None)
            {
                throw new TickestException("Estado de erro inválido.", nameof(error));
            }

            IsSuccess = isSuccess;
            Error = error;
        }
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; }
        public static Result Success() => new(true, Error.None);
        public static Result<TValue> Success<TValue>(TValue value) =>
            new(value, true, Error.None);
        public static Result Failure(Error error) => new(false, error);
        public static Result<TValue> Failure<TValue>(Error error) =>
            new(default, false, error);

        public static Result<Guid> Success(object id)
        {
            throw new NotImplementedException();
        }
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
            new(default, false, error);
    }
}
