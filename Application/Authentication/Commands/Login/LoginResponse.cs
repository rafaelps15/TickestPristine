using Tickest.Domain.Contracts.Responses;

namespace Tickest.Application.Authentication.Commands.Login;

public record LoginResponse(string Token) : IResponse;
