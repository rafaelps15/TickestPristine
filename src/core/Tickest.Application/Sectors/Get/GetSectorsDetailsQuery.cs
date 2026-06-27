using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Sectors.Get;

public sealed record GetSectorsDetailsQuery(Guid SectorId) : IQuery<List<SectorResponse>>;


