using Tickest.Domain.Contracts.Responses;

public record TokenResponse : IApiResponse
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Message { get; } = "Token gerado com sucesso.";
    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }
    public DateTime ExpiresAt { get; init; }

    // Construtor para inicializar todas as propriedades necessárias
    public TokenResponse(string accessToken, string refreshToken, DateTime expiresAt)
    {
        AccessToken = ValidarAccessToken(accessToken);
        RefreshToken = ValidarRefreshToken(refreshToken);
        ExpiresAt = ValidarDataExpiracao(expiresAt);
    }

    private static string ValidarAccessToken(string accessToken)
    {
        return string.IsNullOrWhiteSpace(accessToken)
            ? throw new ArgumentNullException(nameof(accessToken), "O token de acesso não pode ser nulo ou vazio.")
            : accessToken;
    }

    private static string ValidarRefreshToken(string refreshToken)
    {
        return string.IsNullOrWhiteSpace(refreshToken)
            ? throw new ArgumentNullException(nameof(refreshToken), "O token de atualização não pode ser nulo ou vazio.")
            : refreshToken;
    }

    private static DateTime ValidarDataExpiracao(DateTime expiresAt)
    {
        return expiresAt <= DateTime.UtcNow
            ? throw new ArgumentOutOfRangeException(nameof(expiresAt), "A data de expiração deve ser no futuro.")
            : expiresAt;
    }
}
