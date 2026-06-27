using Tickest.SharedKernel;

namespace Tickest.Domain.Users
{
    public sealed record UserRegisteredDomainEvent(Guid UserId) : IDomainEvent;
}
