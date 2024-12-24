using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Sector.Get;

public sealed record GetSectorsDetailsQuery(Guid SectorId) : IQuery<List<SectorResponse>>;


