using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Sectors.Get;

public sealed record GetAllSectorsQuery(): IQuery<List<SectorResponse>>;

