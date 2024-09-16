using MediatR;
using Tickest.Domain.Contracts.Models;

namespace Tickest.Application.Authentication.Commands.Login;

public record LoginCommand(string Email, string Senha) 
    : IRequest<TokenModel>;
