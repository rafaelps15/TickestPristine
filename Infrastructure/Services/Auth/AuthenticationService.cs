﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tickest.Domain.Common;
using Tickest.Domain.Contracts.Responses;
using Tickest.Domain.Contracts.Services;
using Tickest.Domain.Entities;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Infrastructure.Configuracoes;

namespace Tickest.Infrastructure.Services.Auth
{
    public class AuthenticationService : IAuthenticationService
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly JwtConfiguracao _jwtConfiguracao;
		private readonly IUserRepository _usuarioRepository;

		public AuthenticationService(IHttpContextAccessor httpContextAccessor, IOptions<JwtConfiguracao> jwtConfiguracao, IUserRepository usuarioRepository)
			=> (_httpContextAccessor, _jwtConfiguracao, _usuarioRepository) = (httpContextAccessor, jwtConfiguracao.Value, usuarioRepository);

		public async Task<TokenResponse> AuthenticateAsync(User usuario)
		{
			if (usuario == null)
			{
				throw new TickestException("A autenticação falhou: o usuário não foi fornecido.");
			}

			var token = await GenerateTokenJwtAsync(usuario);
			return new TokenResponse(token);
		}

		public async Task<Result<string>> RenewTokenAsync(string userId)
		{
			// Verifica se o userId é nulo ou vazio e se pode ser convertido para um inteiro
			if (string.IsNullOrWhiteSpace(userId) || !int.TryParse(userId, out var userIdInt))
				return Result<string>.Failure($"ID de usuário inválido: {userId}.");
			
			var usuario = await _usuarioRepository.ObterUsuarioPorIdAsync(userIdInt);

			if (usuario == null)
				throw new TickestException($"Usuário com ID {userIdInt} não encontrado.");

			var token = await GenerateTokenJwtAsync(usuario);

			// Retorna o token gerado como resultado de sucesso
			return Result<string>.Success(token);
		}


		private async Task<string> GenerateTokenJwtAsync(User usuario)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_jwtConfiguracao.SecretKey);

			var claims = await CreateClaimsAsync(usuario);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.AddMinutes(_jwtConfiguracao.ExpirationInMinutes),
				Issuer = _jwtConfiguracao.Issuer,
				Audience = _jwtConfiguracao.Audience,
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		private async Task<IEnumerable<Claim>> CreateClaimsAsync(User usuario)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Sid, usuario.Id.ToString())
			};

			var usuarioRoles = await _usuarioRepository.ObterRegrasUsuarioAsync(usuario.Id);
			foreach (var usuarioRegra in usuarioRoles)
				claims.Add(new Claim(ClaimTypes.Role, usuarioRegra.Role.Name));
			
			return claims;
		}
	}
}
