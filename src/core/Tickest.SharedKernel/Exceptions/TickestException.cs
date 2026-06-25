namespace Tickest.SharedKernel.Exceptions;

public class TickestException : Exception
{
    public string? ErrorCode { get; }
    public string? Details { get; }

    public TickestException()
        : base("Ocorreu um erro inesperado no sistema.")
    {
    }

    public TickestException(string message)
        : base(message)
    {
    }

    public TickestException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public TickestException(string message, string errorCode, string? details = null)
        : base(message)
    {
        ErrorCode = errorCode;
        Details = details;
    }

    public TickestException(IEnumerable<string> validationErrors)
        : base("Erros de validação encontrados.")
    {
        Details = string.Join("; ", validationErrors);
    }
}
