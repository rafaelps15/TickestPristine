using Tickest.Application.Abstractions.Authentication;
using Tickest.Domain.Interfaces.Repositories;

namespace Infrastructure.Authorization
{
    #region "Provedor de Permissões para Usuários"

    internal sealed class PermissionProvider : IPermissionProvider
    {
        private readonly IUserRepository _userRepository;
        private Dictionary<string, Func<HashSet<string>>> _rolePermissions;

        // O construtor agora inicializa a variável _rolePermissions
        public PermissionProvider(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            InitializeRolePermissions();  
        }

        /// <summary>
        /// Inicializa o dicionário de permissões por função.
        /// </summary>
        private void InitializeRolePermissions()
        {
            // Mapeamento de funções para permissões
            _rolePermissions = new Dictionary<string, Func<HashSet<string>>>
            {
                ["AdminGeral"] = GetAdminGeralPermissions, // AdminGeral tem todas as permissões
                ["AdminSetor"] = GetAdminSetorPermissions,
                ["AdminDepartamento"] = GetAdminDepartamentoPermissions,
                ["AdminArea"] = GetAdminAreaPermissions,
               // ["GestorTickets"] = GetGestorTicketsPermissions,
                ["Colaborador"] = GetColaboradorPermissions,
                ["AnalistaSuporte"] = GetAnalistaSuportePermissions
            };

            //Adiciona a permissão "CreateTicket" para todos os usuários
            foreach (var key in _rolePermissions.Keys.ToList())
            {
                var currentPermissions = _rolePermissions[key].Invoke();
                currentPermissions.Add("CreateTicket"); // Todos terão permissão para criar tickets
                _rolePermissions[key] = () => currentPermissions;
            }
        }

        /// <summary>
        /// Obtém as permissões de um usuário com base nas funções atribuídas.
        /// </summary>
        public async Task<HashSet<string>> GetPermissionsForUserAsync(Guid userId)
        {
            // Obtém as funções do usuário e mapeia para as permissões correspondentes
            //var roles = await _userRepository.GetUserRolesAsync(userId);

            //var roles = new string[1];

            //var permissions = new HashSet<string>();

            //foreach (var role in roles)
            //{
            //    if (_rolePermissions.TryGetValue(role.Description, out var getPermissions))
            //    {
            //        permissions.UnionWith(getPermissions());
            //    }
            //    else
            //    {
            //        // Função desconhecida
            //        Console.WriteLine($"Função desconhecida: {role.Description}");
            //    }
            //}

            //return permissions;

            return null;
        }

        #region "Permissões por Função"

        /// <summary>
        /// Permissões associadas a cada função de usuário.
        /// </summary>
        private static HashSet<string> GetAdminGeralPermissions() {

            // AdminGeral tem acesso completo a todas as funções do sistema
            return new HashSet<string>
            {
                "ControlSystem", "ManageUsers", "ViewReports",
                "ManageDepartments", "DefinePolicies", "MonitorTickets", "AssignRoles",
                "ManageArea", "ViewAreaReports", "ManageTickets", "ViewAllTickets",
                "CreateTicket", "ViewOwnTickets", "ManageAssignedTickets", "ChatWithRequester"
            };
        }

        //Demais regras....


        private static HashSet<string> GetAdminSetorPermissions() => new() { "ManageDepartments", "DefinePolicies" };
        private static HashSet<string> GetAdminDepartamentoPermissions() => new() { "MonitorTickets", "AssignRoles" };
        private static HashSet<string> GetAdminAreaPermissions() => new() { "ManageArea", "ViewAreaReports" };
        //private static HashSet<string> GetGestorTicketsPermissions() => new() { "ManageTickets", "ViewAllTickets" };
        private static HashSet<string> GetColaboradorPermissions() => new() { "CreateTicket", "ViewOwnTickets" };
        private static HashSet<string> GetAnalistaSuportePermissions() => new() { "ManageAssignedTickets", "ChatWithRequester" };

        public Task<HashSet<string>> GetForUserIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    #endregion
}

