using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.DTOs;
using Tickest.Domain.Entities.Users;
using Tickest.Domain.Exceptions;
using Tickest.Infrastructure.Configurations;

namespace Tickest.Infrastructure.Authentication;

public class TokenProvider : ITokenProvider
{
    private readonly JwtConfiguration _jwtConfiguration;

    public TokenProvider(JwtConfiguration jwtConfiguration) =>
        _jwtConfiguration = jwtConfiguration ?? throw new TickestException("Configuração JWT inválida.");

    public TokenResponse GenerateToken(User user)
    {
        var claims = BuildClaims(user);
        var credentials = GetSigningCredentials();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtConfiguration.ExpirationMinutes),
            Issuer = _jwtConfiguration.Issuer,
            Audience = _jwtConfiguration.Audience,
            SigningCredentials = credentials
        };

        var token = new JwtSecurityTokenHandler().CreateToken(tokenDescriptor);
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return new TokenResponse
        {
            AccessToken = tokenString,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtConfiguration.ExpirationMinutes),
            TokenType = "Bearer"
        };
    }

    private List<Claim> BuildClaims(User user)
    {
        return new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.Role, string.Join(",", user.UserRoles))
        };
    }

    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_jwtConfiguration.Secret);
        var signingKey = new SymmetricSecurityKey(key);
        return new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
    }
}
