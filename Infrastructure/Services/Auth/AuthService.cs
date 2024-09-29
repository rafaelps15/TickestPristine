using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Tickest.Domain.Contracts.Models;
using Tickest.Domain.Entities;
using Tickest.Infrastructure.Configuracoes;
using Tickest.Infrastructure.Interfaces;

namespace Tickest.Infrastructure.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JwtConfiguracao _jwtConfiguracao;

        public AuthService(IHttpContextAccessor httpContextAccessor, IOptions<JwtConfiguracao> jwtConfiguracao)
            => (_httpContextAccessor, _jwtConfiguracao) = (httpContextAccessor, jwtConfiguracao.Value);

        public async Task<TokenModel> AuthenticateAsync(Usuario usuario)
        {
            var token = GenerateTokenJwt(usuario);
            return await Task.FromResult(new TokenModel(token));
        }

        private string GenerateTokenJwt(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfiguracao.ChaveSecreta);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.Email),
                    //Exemplos;
                    //new Claim(ClaimTypes.Role, usuario.Role), 
                    //new Claim(ClaimTypes.Email, usuario.Email),
                    //new Claim("UserId", usuario.Id.ToString()), 
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
}
