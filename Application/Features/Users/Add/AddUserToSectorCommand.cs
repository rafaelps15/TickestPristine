using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Features.Users.Add;

public record AddUserToSectorCommand(
    Guid SectorId, 
    Guid UserId
):ICommand<Guid>;
