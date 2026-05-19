namespace Tickest.Application.Roles.GetAll;

public sealed record RoleResponse(
    Guid Id,
    string Name,
    string Description
);
