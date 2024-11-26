using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Contracts.Responses.User;

namespace Tickest.Application.Users.Create
{
    public class CreateUserCommand : ICommand<CreateUserResponse>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public List<string> RoleNames { get; set; }  // Lista de roles a serem atribuídas
        public string Sector { get; set; }  // Setor
        public string Department { get; set; }  // Departamento
        public string Area { get; set; }  // Área
        public string Specialty { get; set; }  // Especialidade
    }

}
