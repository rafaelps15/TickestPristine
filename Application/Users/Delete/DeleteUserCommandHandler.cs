using MediatR;
using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Contracts.Responses.User;
using Tickest.Domain.Entities;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Users.Delete
{
    public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, DeleteUserResponse>
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
            // Validação do comando de exclusão
            _logger.LogInformation("Validação da solicitação de exclusão de usuário concluída com sucesso.");

            // Busca o usuário
            var user = await GetUserAsync(request.UserId);

            // Deleta o usuário
            await _baseRepository.DeleteByIdAsync(user.Id);
            _logger.LogInformation($"Usuário com ID {user.Id} deletado com sucesso.");

            // Retorna a resposta com os dados do usuário deletado
            return new DeleteUserResponse(user.Id, user.Email, user.Name);
        }

        private async Task<User> GetUserAsync(Guid userId)
        {
            var user = await _baseRepository.GetByIdAsync(userId);
            if (user == null)
                throw new TickestException($"Usuário com ID {userId} não encontrado.");

            return user;
        }
    }
}