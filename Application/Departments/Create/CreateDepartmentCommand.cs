using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Departments.Create;

public record CreateDepartmentCommand(
    string Name,
    string Description,
    Guid? DepartmentManagerId,
    Guid SectorId
) : ICommand<Guid>; // Retorna o ID do Departamento criado
