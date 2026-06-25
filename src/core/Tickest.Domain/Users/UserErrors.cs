using Tickest.SharedKernel;

namespace Tickest.Domain.Users;

public static class UserErrors
{
    public static Error NotFound(Guid userId) => Error.NotFound(
        "Users.NotFound",
        $"Usuário com Id '{userId}' não encontrado.");

    public static Error Unauthorized() => Error.Failure(
        "Users.Unauthorized",
        "Você não tem autorização para executar esta ação.");

    public static readonly Error NotFoundByEmail = Error.NotFound(
        "Users.NotFoundByEmail",
        "Usuário com o e-mail informado não encontrado.");

    public static readonly Error EmailNotUnique = Error.Conflict(
        "Users.EmailNotUnique",
        "O e-mail informado já está em uso.");
}
