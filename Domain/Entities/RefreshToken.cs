using System;
using Tickest.Domain.Entities.Base;

namespace Tickest.Domain.Entities.Auths
{
    /// <summary>
    /// RefreshToken: Representa um token de atualização usado para obter novos tokens de acesso.
    /// </summary>
    public class RefreshToken : EntityBase
    {
        public Guid UserId { get; set; } // Identificador do usuário ao qual o token pertence
        public string Token { get; set; } // Token de atualização gerado
        public DateTimeOffset ExpiresAt { get; set; } // Data e hora de expiração do token de atualização
        public bool IsRevoked { get; set; } // Indica se o token foi revogado
        public bool IsUsed { get; set; } // Indica se o token foi usado

        /// <summary>
        /// Verifica se o token de atualização é válido.
        /// Um token é válido se não foi usado, não foi revogado e não expirou.
        /// </summary>
        /// <returns>Retorna true se o token for válido, caso contrário, false.</returns>
        public bool IsValid() => !IsUsed && !IsRevoked && DateTimeOffset.UtcNow < ExpiresAt;
    }
}
