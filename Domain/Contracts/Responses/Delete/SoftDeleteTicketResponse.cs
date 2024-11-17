namespace Tickest.Domain.Contracts.Responses.Delete;

public class SoftDeleteTicketResponse
{
    public bool IsDeleted { get; set; }
    public string Message { get; set; }
}
