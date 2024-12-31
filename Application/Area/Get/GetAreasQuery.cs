using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Area.Get;

public sealed record GetAreasQuery(Guid UserId, List<Guid> AreaIds) : IQuery<List<AreaResponse>>;
