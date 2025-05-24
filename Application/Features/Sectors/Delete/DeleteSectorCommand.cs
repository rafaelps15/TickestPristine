using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Features.Sectors.Delete;

public record  DeleteSectorCommand(Guid Id ) : ICommand<Guid>;

