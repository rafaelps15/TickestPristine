namespace Tickest.Domain.Exceptions;


public class TickestException : Exception
{
    // Propriedades auto-implementadas com inicialização
    public string ErrorCode { get; init; }
    public string Details { get; init; }

    public TickestException(string message) : base(message) { }

    public TickestException(string message, Exception innerException)
        : base(message, innerException) { }

    public TickestException()
        : base("Ocorreu um erro inesperado no sistema.") { }

    public TickestException(string message, string errorCode, string details = null)
        : base(message)
    {
        ErrorCode = errorCode;
        Details = details;
    }

    public TickestException(IEnumerable<string> validationErrors)
        : base("Erros de validação encontrados.")
    {
        // Concatena os erros de validação em uma única string
        Details = string.Join("; ", validationErrors);
    }

    public override string ToString()
    {
        var baseString = base.ToString();
        return string.IsNullOrEmpty(ErrorCode) ? baseString : $"{baseString}, Código de Erro: {ErrorCode}, Detalhes: {Details}";
    }
}
