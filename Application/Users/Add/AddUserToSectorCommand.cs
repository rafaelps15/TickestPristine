using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Users.Add;

public record AddUserToSectorCommand(
    Guid SectorId, 
    Guid UserId
):ICommand<Guid>;
