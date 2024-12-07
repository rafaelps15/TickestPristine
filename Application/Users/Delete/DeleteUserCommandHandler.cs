using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Entities.Users;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Application.Abstractions.Authentication;

namespace Tickest.Application.Users.Delete;

public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, Guid>
{
    private readonly IGenericRepository<User> _genericRepository;
    private readonly ILogger<DeleteUserCommandHandler> _logger;
    private readonly IAuthService _authService;

    public DeleteUserCommandHandler(
        IGenericRepository<User> genericRepository,
        ILogger<DeleteUserCommandHandler> logger,
        IAuthService authService) =>
        (_genericRepository, _logger, _authService) = (genericRepository, logger, authService);

    public async Task<Guid> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        // Validação do comando de exclusão
        _logger.LogInformation("Iniciando solicitação de exclusão de usuário.");

        // Verificar o usuário atual (caso a exclusão dependa de permissões)
        var currentUser = await _authService.GetCurrentUserAsync(cancellationToken);

        // Validar permissões do usuário atual para excluir o outro usuário
        // (lógica para verificar se o currentUser tem permissão para deletar o usuário solicitado)

        // Busca o usuário a ser deletado
        var userToDelete = await GetUserAsync(request.UserId, cancellationToken);

        // Verifique se o currentUser tem permissão para excluir esse usuário
        // (Adicionar verificações de permissão)

        // Deleta o usuário
        await _genericRepository.DeleteByIdAsync(userToDelete.Id, cancellationToken);
        _logger.LogInformation($"Usuário com ID {userToDelete.Id} deletado com sucesso.");

        // Retorna o ID do usuário deletado
        return userToDelete.Id;
    }

    private async Task<User> GetUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _genericRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null)
            throw new TickestException($"Usuário com ID {userId} não encontrado.");

        return user;
    }
}
