//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using System.Threading;
//using Tickest.Application.Abstractions.Authentication;
//using Tickest.Application.Abstractions.Messaging;
//using Tickest.Domain.Contracts.Responses.User;
//using Tickest.Domain.Entities;
//using Tickest.Domain.Exceptions;
//using Tickest.Domain.Interfaces.Repositories;

//namespace Tickest.Application.Users.Update;

//public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, UpdateUserResponse>
//{
//    private readonly IUserRepository _userRepository;
//    private readonly IConfiguration _configuration;
//    private readonly IPasswordHasher _passwordHasher;
//    private readonly ILogger<UpdateUserCommandHandler> _logger;

//    public UpdateUserCommandHandler(
//        IUserRepository usuarioRepository,
//        IConfiguration configuration,
//        IPasswordHasher hasherDeSenha,
//        ILogger<UpdateUserCommandHandler> logger)
//        => (_userRepository, _configuration, _passwordHasher, _logger) = (usuarioRepository, configuration, hasherDeSenha, logger);

//    public async Task<UpdateUserResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
//    {
//        var user = await _userRepository.GetByIdAsync(request.UserId,cancellationToken);
//        if (user is null)
//            throw new TickestException($"Usuário com ID {request.UserId} não encontrado.");

//        UpdateUserProperties(user, request);
//        await _userRepository.UpdateAsync(user,cancellationToken);

//        _logger.LogInformation($"Usuário com ID {user.Id} atualizado com sucesso.");
//        return MapResponse(user);
//    }

//    private async Task<User> GetUserAsync(Guid userId, CancellationToken cancellationToken)
//    {
//        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
//        return user ?? throw new TickestException($"Usuário com ID {userId} não encontrado.");
//    }

//    private void UpdateUserProperties(User usuario, UpdateUserCommand request)
//    {
//        usuario.Email = request.Email;
//        usuario.Name = request.Name;
//    }

//    private UpdateUserResponse MapResponse(User user) =>
//        new UpdateUserResponse(user.Id, user.Email, user.Name);
//}