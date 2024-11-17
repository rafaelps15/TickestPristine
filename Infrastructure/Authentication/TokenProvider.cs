using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Domain.Entities;

namespace Infrastructure.Authentication;

internal sealed class TokenProvider : ITokenProvider
{
    private readonly IConfiguration _configuration;

    public TokenProvider(IConfiguration configuration) => 
        _configuration = configuration;

    #region Criar Token
    /// <summary>
    /// Cria um token JWT para o usuário.
    /// </summary>
    public string Create(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role) // Adiciona os dados do usuário como claims
            }),
            Expires = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:ExpirationInMinutes")),// Define a expiração do token
            SigningCredentials = credentials, // Credenciais para assinar o token
            Issuer = _configuration["Jwt:Issuer"],// Emissor do token
            Audience = _configuration["Jwt:Audience"]// Público do token
        };

        return new JsonWebTokenHandler().CreateToken(tokenDescriptor); // Cria o token e o retorna
    }
    #endregion
}
