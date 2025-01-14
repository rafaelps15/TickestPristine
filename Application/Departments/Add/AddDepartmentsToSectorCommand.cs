﻿using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Entities.Sectors;

namespace Tickest.Application.Departments.Add;

public record AddDepartmentsToSectorCommand(
    Guid SectorId,
    Guid DepartamentId
) : ICommand<Guid>;
