using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Users.Create;

public record RegisterUserCommand(
    string Name,
    string EmployeeCode,
    string Email,
    string Password,
    // Temporario para teste: voltar a obrigar setor e especialidades no cadastro final.
    Guid? SectorId = null,
    IReadOnlyCollection<Guid>? SpecialtyIds = null
) : ICommand<Guid>
{ }


