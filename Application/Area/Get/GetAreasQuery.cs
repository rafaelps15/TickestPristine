using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Area.Get
{
    public record GetAreasQuery(Guid UserId) : IQuery<List<AreaResponse>>;
}
