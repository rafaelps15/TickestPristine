using Tickest.Application.Abstractions.Messaging;
using Tickest.Application.Sectors.Get;

namespace Tickest.Application.Sectors.GetById;

public sealed record GetByIdSectorQuery(Guid id) : IQuery<SectorResponse>;


