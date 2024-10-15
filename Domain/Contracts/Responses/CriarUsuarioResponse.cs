namespace Tickest.Domain.Contracts.Responses;

public record CriarUsuarioResponse(int Id, string Email, string Nome) : IResponse;
public record AtualizarUsuarioResponse(int Id, string Email, string Nome) : IResponse;