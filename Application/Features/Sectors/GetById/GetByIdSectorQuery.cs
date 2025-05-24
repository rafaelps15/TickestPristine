using Tickest.Application.Abstractions.Messaging;
using Tickest.Application.Features.Sectors.Get;

namespace Tickest.Application.Features.Sectors.GetById;

public sealed record GetByIdSectorQuery(Guid id) : IQuery<SectorResponse>;


