using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Contracts.Responses.User;
using Tickest.Domain.Entities;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Users.Delete;

public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, DeleteUserResponse>
{
    private readonly IGenericRepository<User> _genericRepository;
    private readonly ILogger<DeleteUserCommandHandler> _logger;

    public DeleteUserCommandHandler(
        IGenericRepository<User> genericRepository,
        ILogger<DeleteUserCommandHandler> logger)
        => (_genericRepository, _logger) = (genericRepository, logger);

    public async Task<DeleteUserResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        // Validação do comando de exclusão
        _logger.LogInformation("Iniciando solicitação de exclusão de usuário.");

        // Busca o usuário
        var user = await GetUserAsync(request.UserId, cancellationToken);

        // Deleta o usuário
        await _genericRepository.DeleteByIdAsync(user.Id, cancellationToken);
        _logger.LogInformation($"Usuário com ID {user.Id} deletado com sucesso.");

        // Retorna a resposta com os dados do usuário deletado
        return new DeleteUserResponse(user.Id, user.Email, user.Name);
    }

    private async Task<User> GetUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _genericRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null)
            throw new TickestException($"Usuário com ID {userId} não encontrado.");

        return user;
    }
}
