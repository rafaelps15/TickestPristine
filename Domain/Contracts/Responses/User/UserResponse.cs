using Tickest.Domain.Entities;

namespace Tickest.Domain.Contracts.Responses.User;

public sealed record UserResponse : IApiResponse
{
    public Guid Id { get; init; }
    public string Message { get; init; }
    public string Name { get; init; }
    public IEnumerable<Role> Roles { get; init; }

    // Construtor principal com parâmetros essenciais
    public UserResponse(Guid id, string name, IEnumerable<Role> roles, string message) =>
        (Id, Name, Roles, Message) = (id, name, roles, message);

    // Construtor alternativo
    public UserResponse(Guid id, string name) : this(id, name, Enumerable.Empty<Role>(), string.Empty) { }
}

// Método auxiliar para criação do UserResponse
public static class UserResponseFactory
{
    // Cria um UserResponse a partir de UserRoles
    public static UserResponse CreateFromUserRoles(Guid id, string name, string message, IEnumerable<UserRole> userRoles) =>
        new(id, name, userRoles?.Select(ur => ur.Role) ?? Enumerable.Empty<Role>(), message);

    // Cria um UserResponse a partir de nomes de roles
    public static UserResponse CreateFromRoleNames(Guid id, string name, string message, IEnumerable<string>? roleNames) =>
        new(id, name, roleNames?.Select(name => new Role { Name = name }) ?? Enumerable.Empty<Role>(), message);
}
