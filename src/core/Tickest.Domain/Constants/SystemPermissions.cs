namespace Tickest.Domain.Constants;

public static class SystemPermissions
{
    public const string AccessSystem = nameof(AccessSystem);
    public const string FullSystemControl = nameof(FullSystemControl);
    public const string AccessCriticalSettings = nameof(AccessCriticalSettings);
    public const string ManageUsers = nameof(ManageUsers);
    public const string DeleteUser = nameof(DeleteUser);
    public const string ManagePermissions = nameof(ManagePermissions);
    public const string ViewReports = nameof(ViewReports);

    public const string CreateTicket = nameof(CreateTicket);
    public const string ViewTicket = nameof(ViewTicket);
    public const string UpdateOwnTicket = nameof(UpdateOwnTicket);
    public const string ManageTickets = nameof(ManageTickets);
    public const string AssignTicket = nameof(AssignTicket);
    public const string ChangeTicketStatus = nameof(ChangeTicketStatus);
    public const string CloseTicket = nameof(CloseTicket);
    public const string ReopenTicket = nameof(ReopenTicket);
    public const string DeleteTicket = nameof(DeleteTicket);
    public const string InteractWithTicket = nameof(InteractWithTicket);
    public const string TrackTicketStatus = nameof(TrackTicketStatus);
}
