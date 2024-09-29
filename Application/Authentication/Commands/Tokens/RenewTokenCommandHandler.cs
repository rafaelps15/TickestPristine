using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tickest.Domain.Contracts.Models;
using Tickest.Infrastructure.Configuracoes;
using Tickest.Infrastructure.Interfaces;

namespace Tickest.Application.Authentication.Commands.Tokens;

public class RenewTokenCommandHandler : IRequestHandler<RenewTokenCommand, TokenModel>
{
    private readonly IAuthenticationService _authService;
    private readonly JwtConfiguracao _jwtConfiguracao;
    private readonly ILogger<RenewTokenCommandHandler> _logger;

    public RenewTokenCommandHandler(
        IAuthenticationService authService,
        IOptions<JwtConfiguracao> jwtConfiguracao,
        ILogger<RenewTokenCommandHandler> logger)
        => (_authService, _jwtConfiguracao, _logger) = (authService, jwtConfiguracao.Value, logger);

    public async Task<TokenModel> Handle(RenewTokenCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.UserId))
        {
            throw new ArgumentException("User ID não pode ser nulo ou vazio.", nameof(request.UserId));
        }

        // Tentando renovar o token e lidando com possíveis erros
        try
        {
            var newToken = await _authService.RenewTokenAsync(request.UserId);
            return new TokenModel(newToken);
        }
        catch (Exception ex)
        {
            // Logando o erro
            _logger.LogError(ex, "Erro ao renovar o token para o usuário {UserId}", request.UserId);
            throw new InvalidOperationException("Falha ao renovar o token.", ex);
        }
    }
}
