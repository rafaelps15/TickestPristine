using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Users.Delete;

public record DeleteUserCommand(
     Guid UserId,
     Guid RequestedById
) : ICommand<Guid>
{ }


