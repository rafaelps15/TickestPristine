namespace Tickest.Domain.Exceptions;

public class TickestException : Exception
{
    /// <summary>
    /// Inicializa uma nova instância da exceção com a mensagem fornecida.
    /// </summary>
    public TickestException(string message) : base(message)
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
        : base("Ocorreu um erro inesperado no sistema.") { }
}
