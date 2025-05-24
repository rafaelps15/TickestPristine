using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Features.Users.Delete;

public record DeleteUserCommand(
     Guid UserId,
     Guid RequestedById
) : ICommand<Guid>
{ }


