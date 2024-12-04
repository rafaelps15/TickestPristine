using Tickest.Domain.Entities.Base;

namespace Tickest.Domain.Entities.Security;

#region Permission
/// <summary>
/// Permission: Representa uma permissão associada a um usuário, como "Criar Ticket", "Editar Ticket", etc.
/// </summary>
public class Permission : EntityBase
{
    public string Name { get; set; }
    public string Description { get; set; }
}
#endregion