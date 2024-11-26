using Tickest.Domain.Entities;

namespace Tickest.Domain.Contracts.Responses.User;

public record UserResponse(Guid Id, string Name, IEnumerable<Role> Roles)
{
    private IEnumerable<UserRole> roles;

    public UserResponse(Guid id, string name, IEnumerable<UserRole> roles)
    {
        Id = id;
        Name = name;
        this.roles = roles;
    }
}


