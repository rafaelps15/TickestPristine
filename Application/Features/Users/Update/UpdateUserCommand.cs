using Tickest.Application.Abstractions.Messaging;
using Tickest.Application.DTOs;

namespace Tickest.Application.Users.Update;

public record UpdateUserCommand(string Name, string Email) : ICommand<UserPersonalDto>;
