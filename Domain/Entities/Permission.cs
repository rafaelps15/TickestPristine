using Tickest.Domain.Entities.Base;

namespace Tickest.Domain.Entities.Permissions;

#region Permission
/// <summary>
/// Permission: Representa uma permissão associada a um usuário, como "Criar Ticket", "Editar Ticket", etc.
/// </summary>
public class Permission : EntityBase
{
    public string Description { get; set; }
}
#endregion