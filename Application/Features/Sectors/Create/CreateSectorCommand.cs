﻿using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;

namespace Tickest.Application.Features.Sectors.Create;

public record CreateSectorCommand(
    string Name,
    string Description,
    DateTime CreatedAt,
    Guid? SectorManagerId
) : ICommand<Guid>;

