using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Features.Areas.Create;

public record CreateAreaCommand(
    Guid Id,
    string Name,
    string Description,
    Guid? AreaManagerId
) : ICommand<Guid>; // Para retornar o Id da Área criada.