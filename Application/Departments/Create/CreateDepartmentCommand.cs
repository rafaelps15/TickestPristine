﻿using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Sectors.Create;

public record CreateDepartmentCommand(
    Guid Id,
    string Name,
    string Description,
    Guid? DepartmentManagerId,
    Guid SectorId
) : ICommand<Guid>; // Retorna o ID do Departamento criado
