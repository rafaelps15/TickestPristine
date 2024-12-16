using System.Diagnostics.CodeAnalysis;
using Tickest.Domain.Exceptions;

namespace Tickest.Domain.Common
{
    // Representa o resultado de uma operação sem valor associado.
    public class Result
    {
        // Construtor protegido para garantir que o estado do erro seja consistente com o sucesso
        protected Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None || !isSuccess && error == Error.None)
            {
                throw new TickestException("Estado de erro inválido.", nameof(error));
            }

            IsSuccess = isSuccess;
            Error = error;
        }

        // Indica se a operação foi bem-sucedida
        public bool IsSuccess { get; }

        // Indica se a operação falhou (é o inverso de IsSuccess)
        public bool IsFailure => !IsSuccess;

        // Detalhes sobre o erro, se houver
        public Error Error { get; }

        // Retorna um resultado de sucesso sem valor
        public static Result Success() => new(true, Error.None);

        // Retorna um resultado de sucesso com um valor
        public static Result<TValue> Success<TValue>(TValue value) =>
            new(value, true, Error.None);

        // Retorna um resultado de falha com um erro específico
        public static Result Failure(Error error) => new(false, error);

        // Retorna um resultado de falha com um erro específico, para valores de tipo
        public static Result<TValue> Failure<TValue>(Error error) =>
            new(default, false, error);

      
    }

    // Representa o resultado de uma operação com um valor associado
    public class Result<TValue> : Result
    {
        private readonly TValue? _value;

        // Construtor interno que cria o resultado com um valor
        internal Result(TValue? value, bool isSuccess, Error error)
            : base(isSuccess, error)
        {
            _value = value;
        }

        // Retorna o valor associado, se a operação for bem-sucedida, ou lança uma exceção
        [NotNull]
        public TValue Value => IsSuccess
            ? _value!
            : throw new TickestException("O valor de um resultado com falha não pode ser acessado.");

        // Conversão implícita para criar Result<TValue> a partir de um valor
        public static implicit operator Result<TValue>(TValue? value) =>
            value != null ? Success(value) : Failure<TValue>(Error.NullValue);

        // Cria um resultado de falha com erro de validação
        public static Result<TValue> ValidationFailure(Error error) =>
            new(default, false, error);
    }
}
