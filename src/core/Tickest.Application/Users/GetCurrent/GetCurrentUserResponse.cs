namespace Tickest.Application.Users.GetCurrent;

public sealed record GetCurrentUserResponse(
    Guid Id,
    string Name,
    string Email,
    Guid RoleId,
    IReadOnlyList<string> Specialties,
    IReadOnlyList<string> Permissions
);
