namespace Tickest.Application.Users.GetById;

public record UserResponse(
    Guid Id,
    string Name,
    string Email,
    IReadOnlyList<string> Specialties,
    IReadOnlyList<string> Permissions);


