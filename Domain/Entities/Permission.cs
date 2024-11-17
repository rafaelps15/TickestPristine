namespace Tickest.Domain.Entities
{
    #region Permission
    /// <summary>
    /// Entidade que representa as permissões no sistema.
    /// Cada permissão define um conjunto de ações que podem ser realizadas dentro do sistema.
    /// </summary>
    public class Permission : EntityBase
    {
        public string Name { get; set; } //Nome da permissão, ex: "ManageUsers".
        public string Description { get; set; }// Descrição da permissão, ex: "Gerenciar usuários".

        public List<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
        public List<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
    }
    #endregion
}
