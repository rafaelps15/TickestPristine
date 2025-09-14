using Tickest.Application.Abstractions.Messaging;
using Tickest.Application.DTOs;

namespace Tickest.Application.Features.Users.Auth;

public record AuthenticateUserCommand(
    string Email,
    string Password)
    : ICommand<TokenResponse>;
