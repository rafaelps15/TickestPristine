using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Tickest.Domain.Contracts.Responses.UserResponses;
using Tickest.Domain.Contracts.Services;
using Tickest.Domain.Entities;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Users.Commands.UpdateUserCommand;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UpdateUserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly IPasswordHasher _passwordHasher; 
    private readonly ILogger<UpdateUserCommandHandler> _logger;

    public UpdateUserCommandHandler(
        IUserRepository usuarioRepository,
        IConfiguration configuration,
        IPasswordHasher hasherDeSenha,
        ILogger<UpdateUserCommandHandler> logger)
        => (_userRepository, _configuration, _passwordHasher, _logger) = (usuarioRepository, configuration, hasherDeSenha, logger);

    public async Task<UpdateUserResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        request.Validate();
        var user = await GetUserAsync(request.UsuerId);
        UpdateUserProperties(user, request);
        await _userRepository.UpdateAsync(user);

        _logger.LogInformation($"Usuário com ID {user.Id} atualizado com sucesso.");
        return MapResponse(user);
    }

    private async Task<User> GetUserAsync(int userId)
    {
        var user = await _userRepository.ObterUsuarioPorIdAsync(userId);
        return user ?? throw new TickestException($"Usuário com ID {userId} não encontrado.");
    }

    private void UpdateUserProperties(User usuario, UpdateUserCommand request)
    {
        if (request == null) throw new ArgumentNullException(nameof(request), "O comando de atualização não pode ser nulo.");

        usuario.Email = request.Email;
        usuario.Name = request.Name;

        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            var salt = _passwordHasher.GenerateSalt();
            usuario.Password = _passwordHasher.HashPassword(request.Password, salt);
        }
    }

    private UpdateUserResponse MapResponse(User user) =>
        new UpdateUserResponse(user.Id, user.Email, user.Name);
}