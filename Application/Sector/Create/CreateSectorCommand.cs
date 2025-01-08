using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;

namespace Tickest.Application.Sectors.Create;

public record CreateSectorCommand(
    string Name,
    string Description,
    Guid? SectorManagerId
) : ICommand<Guid>;

