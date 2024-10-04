using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tickest.Domain.Contracts.Responses;
using Tickest.Domain.Contracts.Services;
using Tickest.Domain.Entities;
using Tickest.Domain.Repositories;
using Tickest.Infrastructure.Configuracoes;

namespace Tickest.Infrastructure.Services.Auth;

public class AuthenticationService : IAuthenticationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JwtConfiguracao _jwtConfiguracao;
    private readonly IUsuarioRepository _usuarioRepository;

    public AuthenticationService(IHttpContextAccessor httpContextAccessor, IOptions<JwtConfiguracao> jwtConfiguracao, IUsuarioRepository usuarioRepository)
         => (_httpContextAccessor, _jwtConfiguracao, _usuarioRepository) = (httpContextAccessor, jwtConfiguracao.Value, usuarioRepository);

    public async Task<TokenResponse> AuthenticateAsync(Usuario usuario)
    {
        var token = await GenerateTokenJwt(usuario);
        return new TokenResponse(token);
    }

    public async Task<string> RenewTokenAsync(int userId)
    {
        // Obtendo o usuário pelo ID
        var usuario = await _usuarioRepository.ObterUsuarioPorIdAsync(userId);

        if (usuario == null)
            throw new ArgumentException($"Usuário com ID {userId} não encontrado.");

        // Gerando um novo token para o usuário encontrado
        return await GenerateTokenJwt(usuario);
    }

    public Task<string> RenewTokenAsync(string userId)
    {
        throw new NotImplementedException();
    }

    private async Task<string> GenerateTokenJwt(Usuario usuario)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtConfiguracao.ChaveSecreta);

        var roles = new List<Claim>
        {
            new Claim(ClaimTypes.Sid, usuario.Id.ToString())
        };

        var usuarioRoles = await _usuarioRepository.ObterRegrasUsuarioAsync(usuario.Id);

        foreach (var usuarioRegra in usuarioRoles)
            roles.Add(new Claim(ClaimTypes.Role, usuarioRegra.Regra.Nome));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(roles),
            Expires = DateTime.UtcNow.AddMinutes(_jwtConfiguracao.ExpiracaoEmMinutos),
            Issuer = _jwtConfiguracao.Emissor,
            Audience = _jwtConfiguracao.Audiencia,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
