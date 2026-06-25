using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Domain.Entities.Users;
using Tickest.SharedKernel;

namespace Tickest.Infrastructure.Authentication;

internal sealed class TokenProvider(IOptions<JwtSettings> jwtOptions, IDateTimeProvider dateTimeProvider) : ITokenProvider
{
    private readonly JwtSettings _jwtSettings = jwtOptions.Value;

    public string Create(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
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
            Expires = dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
            SigningCredentials = credentials,
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience
        };

        var handler = new JsonWebTokenHandler();

        string token = handler.CreateToken(tokenDescriptor);

        return token;
    }
}
