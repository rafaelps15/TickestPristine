using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Users.Delete;

public class DeleteUserCommand : ICommand<Guid>
{
    public Guid UserId { get; set; }
    public Guid RequestedById { get; set; }
}
