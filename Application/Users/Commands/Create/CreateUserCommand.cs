using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Contracts.Responses.User;

namespace Tickest.Application.Users.Commands.Create
{
    public class CreateUserCommand : ICommand<CreateUserResponse> 
    {
        #region Properties

        public int Id { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        #endregion
    }
}
