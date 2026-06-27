using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.DTOs;
using Tickest.Domain.Entities.Users;
using Tickest.SharedKernel;
using Tickest.SharedKernel.Exceptions;

namespace Tickest.Infrastructure.Authentication;

internal sealed class TokenProvider(
    IConfiguration configuration,
    IDateTimeProvider dateTimeProvider)
    : ITokenProvider
{
    public TokenResponse Create(User user)
    {
        var jwtSettings = GetJwtSettings();
        var expiresAt = dateTimeProvider.UtcNow.AddMinutes(jwtSettings.ExpirationInMinutes);
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("roleId", user.RoleId.ToString()),
                new Claim(ClaimTypes.Role, user.Role.Name)
            ]),
            Expires = expiresAt,
            SigningCredentials = credentials,
            Issuer = jwtSettings.Issuer,
            Audience = jwtSettings.Audience
        };

        var handler = new JsonWebTokenHandler();
        var token = handler.CreateToken(tokenDescriptor);

        return new TokenResponse
        {
            AccessToken = token,
            ExpiresAt = expiresAt,
            TokenType = "Bearer"
        };
    }

    private JwtSettings GetJwtSettings()
    {
        var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();

        if (jwtSettings is null || string.IsNullOrWhiteSpace(jwtSettings.Secret))
        {
            throw new TickestException("O segredo JWT está ausente ou é nulo na configuração.");
        }

        return jwtSettings;
    }
}
