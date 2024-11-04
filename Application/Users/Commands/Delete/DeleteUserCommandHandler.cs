using MediatR;
using Microsoft.Extensions.Logging;
using Tickest.Domain.Contracts.Responses.UserResponses;
using Tickest.Domain.Entities;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Users.Commands.DeleteUserCommand;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, DeleteUserResponse>
{
    private readonly IBaseRepository<User> _baseRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<DeleteUserCommandHandler> _logger;

    public DeleteUserCommandHandler(
        IUserRepository userRepository,
        IBaseRepository<User> baseRepository,
        ILogger<DeleteUserCommandHandler> logger)
        => (_userRepository, _baseRepository, _logger) = (userRepository, baseRepository, logger);

    public async Task<DeleteUserResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        // Validação da solicitação de exclusão de usuário
        request.Validate();
        _logger.LogInformation("Validação da solicitação de exclusão de usuário concluída com sucesso.");

        var user = await GetUserAsync(request.UserId)
            ?? throw new TickestException($"Usuário com ID {request.UserId} não encontrado.");

        // Deleta o usuário
        await _baseRepository.DeleteByIdAsync(user.Id, cancellationToken);
        _logger.LogInformation($"Usuario com ID {user.Id} deletado com sucesso.");

        // Retorna a resposta com o ID do usuário excluído
        return new DeleteUserResponse(user.Id,user.Email,user.Name);
    }

    private async Task<User> GetUserAsync(int userId)
    {
        var user = await _userRepository.ObterUsuarioPorIdAsync(userId);
        return user ?? throw new TickestException($"Usuário com ID {userId} não encontrado.");
    }

}
