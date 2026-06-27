using Tickest.Domain.Entities.Base;
using Tickest.SharedKernel;
using Tickest.SharedKernel.Exceptions;

namespace Tickest.Domain.Entities.Auths;

public class RefreshToken : AuditableEntity
{
    public EntityId UserId { get; private set; } = null!;
    public string Token { get; private set; } = null!;
    public DateTime ExpiresAt { get; private set; }
    public bool IsRevoked { get; private set; }
    public bool IsUsed { get; private set; }

    private RefreshToken()
    {
    }

    public bool IsValid(DateTime utcNow) => !IsUsed && !IsRevoked && utcNow < ExpiresAt;

    public void Revoke(DateTime utcNow)
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
        MarkAsUpdated(utcNow);
    }

    public void MarkAsUsed(DateTime utcNow)
    {
        if (IsUsed)
        {
            throw new TickestException("O token já foi usado.");
        }

        IsUsed = true;
        MarkAsUpdated(utcNow);
    }
}
