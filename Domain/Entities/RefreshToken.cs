using Tickest.Domain.Entities.Base;
using Tickest.Domain.Exceptions;

namespace Tickest.Domain.Entities.Auths;

public class RefreshToken : EntityBase
{
    public Guid UserId { get; set; }
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }
    public bool IsUsed { get; set; }

    public bool IsValid() => !IsUsed && !IsRevoked && DateTime.UtcNow < ExpiresAt;

    public void Revoke()
    {
        if (IsUsed)
        {
            throw new TickestException("O token já foi utilizado e não pode ser revogado.");
        }

        if (IsRevoked)
        {
            throw new TickestException("O token já foi revogado.");
        }

        IsRevoked = true;
        UpdateAt = DateTime.UtcNow;
    }

    public void MarkAsUsed()
    {
        if (IsUsed)
        {
            throw new TickestException("O token já foi usado.");
        }

        IsUsed = true;
        UpdateAt = DateTime.UtcNow;
    }
}
