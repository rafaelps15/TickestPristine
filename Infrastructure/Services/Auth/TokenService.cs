using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tickest.Infrastructure.Configuracoes;
using Tickest.Domain.Contracts.Services;

namespace Tickest.Infrastructure.Services.Authentication
{
    public class TokenService : ITokenService
    {
        private readonly JwtConfiguracao _jwtConfiguracao;
        private readonly Dictionary<string, string> _refreshTokens;

        public TokenService(JwtConfiguracao jwtConfiguracao)
            => (_jwtConfiguracao, _refreshTokens) = (jwtConfiguracao, new Dictionary<string, string>());

        public async Task<string> GerarTokenAsync(string userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfiguracao.SecretKey);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
                // Exemplos
                // new Claim(ClaimTypes.Role, usuario.Role ?? string.Empty), 
                // new Claim(ClaimTypes.Email, usuario.Email),
                // new Claim("UserId", usuario.Id.ToString()),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtConfiguracao.TokenExpirationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = GerarRefreshToken(userId);
            _refreshTokens[refreshToken] = userId;

            return await Task.FromResult(tokenHandler.WriteToken(token));
        }

        public async Task<string> RenovarTokenAsync(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken) || !EhRefreshTokenValido(refreshToken))
            {
                throw new InvalidOperationException("Refresh token inválido.");
            }

            var userId = ObterUserIdDoRefreshToken(refreshToken);
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new InvalidOperationException("Não foi possível obter o usuário a partir do refresh token.");
            }

            return await GerarTokenAsync(userId);
        }

        private bool EhRefreshTokenValido(string refreshToken)
            => _refreshTokens.ContainsKey(refreshToken);

        private string ObterUserIdDoRefreshToken(string refreshToken)
            => _refreshTokens.TryGetValue(refreshToken, out var userId) ? userId : null;

        private string GerarRefreshToken(string userId)
            => Guid.NewGuid().ToString(); // Gera um novo refresh token
    }
}
