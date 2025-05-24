using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Features.Sectors.Get;

public sealed record GetAllSectorsQuery(): IQuery<List<SectorResponse>>;

