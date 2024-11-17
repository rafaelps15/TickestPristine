using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Contracts.Responses.User;

namespace Tickest.Application.Users.Commands.Delete;

public class DeleteUserCommand : ICommand<DeleteUserResponse>
{
    public Guid UserId { get; set; }
    public Guid RequestedById { get; set; }
}
