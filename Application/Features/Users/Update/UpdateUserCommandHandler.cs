using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Application.DTOs;
using Tickest.Application.Users.Update;
using Tickest.Domain.Common;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Features.Users.Update;

internal sealed class UpdateUserCommandHandler(
IAuthService authService,
IUserRepository userRepository
) : ICommandHandler<UpdateUserCommand, UserPersonalDto>
{
    public async Task<Result<UserPersonalDto>> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var userId = authService.GetCurrentUserId();

        var user = await userRepository.GetByIdAsync(userId, cancellationToken)
                   ?? throw new TickestException("Usuário não encontrado.");

        user.Name = command.Name;
        user.Email = command.Email;

        await userRepository.UpdateAsync(user, cancellationToken);

        var dto = new UserPersonalDto(user.Id, user.Name, user.Email);
        return Result.Success(dto);
    }
}
