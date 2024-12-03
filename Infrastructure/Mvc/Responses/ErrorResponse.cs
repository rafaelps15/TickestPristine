namespace Tickest.Infrastructure.Mvc.Responses;

public record ErrorResponse(string Code, string Message, string DetailedMessage, string CorrelationId, DateTime Timestamp);
