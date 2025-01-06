using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Departments.Get;

public sealed record GetSectorsDetailsQuery(Guid UserId, List<Guid> SectorIds) : IQuery<List<SectorResponse>>;


