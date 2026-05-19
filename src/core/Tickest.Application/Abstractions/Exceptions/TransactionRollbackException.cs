namespace Tickest.Application.Abstractions.Exceptions;

public class TransactionRollbackException : Exception
{
    public TransactionRollbackException(string message)
        : base(message) { }
}
