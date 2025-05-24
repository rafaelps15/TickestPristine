using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Features.Departments.Create;

public record CreateDepartmentCommand(
    Guid Id,
    string Name,
    string Description,
    Guid? DepartmentManagerId
) : ICommand<Guid>; // Retorna o ID do Departamento criado
