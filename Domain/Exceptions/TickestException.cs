//namespace Tickest.Domain.Exceptions;

//public class TickestException : Exception
//{
//    /// <summary>
//    /// Inicializa uma nova instância da exceção com a mensagem fornecida.
//    /// </summary>
//    public TickestException(string message) : base(message)
//    {
//    }

//    /// <summary>
//    /// Inicializa uma nova instância da exceção com a mensagem e a exceção interna fornecidas.
//    /// </summary>
//    public TickestException(string message, Exception innerException) 
//        : base(message, innerException)
//    {
//    }

//    /// <summary>
//    /// Inicializa uma nova instância da exceção sem mensagem.
//    /// </summary>
//    public TickestException()
//        : base("Ocorreu um erro inesperado no sistema.") { }
//}
namespace Tickest.Domain.Exceptions
{
    /// <summary>
    /// Exceção personalizada para o sistema Tickest, utilizada para encapsular erros específicos do domínio.
    /// </summary>
    public class TickestException : Exception
    {
        // Propriedades auto-implementadas com inicialização
        public string ErrorCode { get; init; }
        public string Details { get; init; }

        /// <summary>
        /// Inicializa uma nova instância da exceção com a mensagem fornecida.
        /// </summary>
        public TickestException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Inicializa uma nova instância da exceção com a mensagem e a exceção interna fornecidas.
        /// </summary>
        public TickestException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Inicializa uma nova instância da exceção sem mensagem.
        /// </summary>
        public TickestException()
            : base("Ocorreu um erro inesperado no sistema.")
        { }

        /// <summary>
        /// Inicializa uma nova instância da exceção com a mensagem, código de erro e detalhes opcionais.
        /// </summary>
        public TickestException(string message, string errorCode, string details = null)
            : base(message)
        {
            ErrorCode = errorCode;
            Details = details;
        }

        /// <summary>
        /// Inicializa uma nova instância da exceção com uma lista de erros de validação formatados como string.
        /// </summary>
        public TickestException(IEnumerable<string> validationErrors)
            : base("Erros de validação encontrados.")
        {
            // Concatena os erros de validação em uma única string
            Details = string.Join("; ", validationErrors);
        }

        /// <summary>
        /// Sobrescreve o método ToString para incluir o código de erro e detalhes adicionais, se fornecidos.
        /// </summary>
        public override string ToString()
        {
            var baseString = base.ToString();
            return string.IsNullOrEmpty(ErrorCode) ? baseString : $"{baseString}, Código de Erro: {ErrorCode}, Detalhes: {Details}";
        }
    }
}
