using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Features.Areas.Get;

public sealed record GetAreasQuery(Guid UserId, List<Guid> AreasId) : IQuery<List<AreaResponse>>;
