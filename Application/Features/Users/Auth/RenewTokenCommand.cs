using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Features.Users.Auth;

public record RenewTokenCommand(string RefreshToken) : ICommand<string>;
