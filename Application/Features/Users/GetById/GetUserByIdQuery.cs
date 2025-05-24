using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Features.Users.GetById;

public sealed record GetUserByIdQuery(Guid UserId) : IQuery<UserResponse>;