namespace Tickest.Domain.Entities;

public class RefreshToken : EntityBase
{
    public Guid UserId { get; private set; }
    public string Token { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public bool IsActive { get; private set; }

    // Propriedade de navegação para o usuário associado ao token
    public User User { get; set; } 

    // Construtor para criação de um novo RefreshToken
    public RefreshToken(Guid userId, string token, DateTime expiresAt)
    {
        UserId = userId;
        Token = token ?? throw new ArgumentNullException(nameof(token), "Token cannot be null.");
        ExpiresAt = expiresAt;
        IsActive = true;
    }

    // Método para invalidar o token
    public void Invalidate()
    {
        IsActive = false;
    }

    // Método que verifica se o token expirou
    public bool HasExpired() => DateTime.UtcNow >= ExpiresAt;
}
