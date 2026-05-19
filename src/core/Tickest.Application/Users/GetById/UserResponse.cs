namespace Tickest.Application.Users.GetById;

public sealed record UserResponse(
    Guid Id,
    string Name,
    string Email,
    Guid RoleId,
    string Role,
    IReadOnlyList<string> Specialties,
    IReadOnlyList<string> Permissions
);


