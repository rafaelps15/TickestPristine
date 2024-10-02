using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tickest.Domain.Contracts.Models;
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

    public async Task<TokenModel> AuthenticateAsync(Usuario usuario)
    {
        var token = GenerateTokenJwt(usuario);
        return await Task.FromResult(new TokenModel(token));
    }

    public async Task<string> RenewTokenAsync(int userId)
    {
        // Obtendo o usuário pelo ID
        var usuario = await _usuarioRepository.ObterUsuarioPorIdAsync(userId);

        if (usuario == null)
            throw new ArgumentException($"Usuário com ID {userId} não encontrado.");

        // Gerando um novo token para o usuário encontrado
        return GenerateTokenJwt(usuario);
    }

    public Task<string> RenewTokenAsync(string userId)
    {
        throw new NotImplementedException();
    }

    private string GenerateTokenJwt(Usuario usuario)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtConfiguracao.ChaveSecreta);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, usuario.Email),
                // Você pode descomentar as linhas abaixo se precisar incluir mais informações no token
                // new Claim(ClaimTypes.Role, usuario.Role ?? string.Empty), 
                // new Claim(ClaimTypes.Email, usuario.Email),
                // new Claim("UserId", usuario.Id.ToString()),
            }),
            Expires = DateTime.UtcNow.AddMinutes(_jwtConfiguracao.ExpiracaoEmMinutos),
            Issuer = _jwtConfiguracao.Emissor,
            Audience = _jwtConfiguracao.Audiencia,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
