
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Domain.Common;
using Tickest.Domain.Contracts.Responses;
using Tickest.Domain.Entities;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Infrastructure.Configurations;

namespace Tickest.Infrastructure.Authentication;

internal class AuthenticationService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtConfiguration _jwtConfiguracao;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthenticationService(
        IUserRepository userRepository,
        IOptions<JwtConfiguration> jwtConfiguracao,
        IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _jwtConfiguracao = jwtConfiguracao.Value;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TokenResponse> AuthenticateAsync(User user)
    {
        var token = GenerateTokenJwt(user);
        return await Task.FromResult(new TokenResponse(token));
    }

    private string GenerateTokenJwt(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtConfiguracao.SecretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString(), ClaimValueTypes.String, "ticket-auth"),
                new Claim(ClaimTypes.Name, user.Email),
                //Exemplos;
                //new Claim(ClaimTypes.Role, usuario.Role), 
                //new Claim(ClaimTypes.Email, usuario.Email),
                //new Claim("UserId", usuario.Id.ToString()), 
            }),
            Expires = DateTime.UtcNow.AddMinutes(_jwtConfiguracao.ExpirationMinutes),
            Issuer = _jwtConfiguracao.Issuer,
            Audience = _jwtConfiguracao.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<User> GetCurrentUserAsync()
    {
        var authenticationResult = await _httpContextAccessor.HttpContext.AuthenticateAsync("Bearer");
        if (!authenticationResult.Succeeded)
            return null;

        var userId = new Guid(authenticationResult.Principal.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier).Value);

        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
            return null;

        if (!user.IsActive)
            throw new InvalidOperationException("User is not active.");

        return user;
    }

    public async Task<Result<string>> RenewTokenAsync(string refreshToken)
    {
        return null;
    }
}
