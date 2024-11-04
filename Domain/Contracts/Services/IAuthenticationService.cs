using Tickest.Domain.Common;
using Tickest.Domain.Contracts.Responses;
using Tickest.Domain.Entities;

namespace Tickest.Domain.Contracts.Services;

public interface IAuthenticationService
{
	Task<TokenResponse> AuthenticateAsync(User usuario);
	Task<Result<string>> RenewTokenAsync(string userId); // Método atualizado
}
