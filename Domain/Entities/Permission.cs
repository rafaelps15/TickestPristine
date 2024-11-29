
namespace Tickest.Domain.Entities
{
    #region Permission

    public class Permission : EntityBase
    {
        public string Name { get; set; } //Nome da permissão, ex: "ManageUsers".
        public string Description { get; set; }// Descrição da permissão, ex: "Gerenciar usuários".

        //public ICollection<Permission> Permissions { get; private set; }
        public ICollection<UserPermission> UserPermissions { get; private set; }
    }
    #endregion
}
