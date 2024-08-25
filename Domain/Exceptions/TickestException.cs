namespace Tickest.Domain.Exceptions;

public class TickestException : Exception
{
    public TickestException(string message) : base(message)
    {
        
    }

    public TickestException(string message, Exception exception) : base(message, exception)
    {

    }
}
