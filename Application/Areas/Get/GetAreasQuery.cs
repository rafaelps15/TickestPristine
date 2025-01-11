using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Departments.Get;

public sealed record GetAreasQuery(Guid UserId, List<Guid> AreasId) : IQuery<List<AreaResponse>>;
