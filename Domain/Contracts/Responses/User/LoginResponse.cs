namespace Tickest.Domain.Contracts.Responses.User;

public record LoginResponse(
    Guid Id,
    string Email,
    string Name,
    TokenResponse Token,
    IEnumerable<string> Roles,
    string Message = "Login realizado com sucesso.") : IApiResponse
{ }



