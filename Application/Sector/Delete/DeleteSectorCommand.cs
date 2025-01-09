using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Sectors.Delete;

public record  DeleteSectorCommand(Guid Id ) : ICommand<Guid>;

