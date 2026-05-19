namespace Tickest.Domain.Constants;

public static class SystemRoles
{
    public static readonly Guid AdminMasterId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public static readonly Guid GeneralAdminId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    public static readonly Guid SectorAdminId = Guid.Parse("33333333-3333-3333-3333-333333333333");
    public static readonly Guid DepartmentAdminId = Guid.Parse("44444444-4444-4444-4444-444444444444");
    public static readonly Guid AreaAdminId = Guid.Parse("55555555-5555-5555-5555-555555555555");
    public static readonly Guid TicketManagerId = Guid.Parse("66666666-6666-6666-6666-666666666666");
    public static readonly Guid CollaboratorId = Guid.Parse("77777777-7777-7777-7777-777777777777");
    public static readonly Guid SupportAnalystId = Guid.Parse("88888888-8888-8888-8888-888888888888");

    public const string AdminMaster = nameof(AdminMaster);
    public const string GeneralAdmin = nameof(GeneralAdmin);
    public const string SectorAdmin = nameof(SectorAdmin);
    public const string DepartmentAdmin = nameof(DepartmentAdmin);
    public const string AreaAdmin = nameof(AreaAdmin);
    public const string TicketManager = nameof(TicketManager);
    public const string Collaborator = nameof(Collaborator);
    public const string SupportAnalyst = nameof(SupportAnalyst);

    public static IReadOnlyCollection<RoleDefinition> All { get; } =
    [
        new(AdminMasterId, AdminMaster, "Dono do software."),
        new(GeneralAdminId, GeneralAdmin, "Administrador geral da empresa."),
        new(SectorAdminId, SectorAdmin, "Administrador de setores."),
        new(DepartmentAdminId, DepartmentAdmin, "Administrador de departamentos."),
        new(AreaAdminId, AreaAdmin, "Administrador de areas."),
        new(TicketManagerId, TicketManager, "Gestor de tickets."),
        new(CollaboratorId, Collaborator, "Colaborador solicitante."),
        new(SupportAnalystId, SupportAnalyst, "Analista de suporte.")
    ];

    public sealed record RoleDefinition(Guid Id, string Name, string Description);
}
