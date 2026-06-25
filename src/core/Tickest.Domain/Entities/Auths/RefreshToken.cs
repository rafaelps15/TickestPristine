using Tickest.Domain.Entities.Base;
using Tickest.SharedKernel.Exceptions;

namespace Tickest.Domain.Entities.Auths;

public class RefreshToken : AuditableEntity
{
    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }
    public bool IsUsed { get; set; }

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
