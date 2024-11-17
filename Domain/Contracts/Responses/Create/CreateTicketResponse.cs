namespace Tickest.Domain.Contracts.Responses.Create;

public class CreateTicketResponse
{
    public bool IsCreated { get; set; }
    public string Message { get; set; }
    public Guid TicketId { get; set; }
    public bool Success { get; set; }
}
