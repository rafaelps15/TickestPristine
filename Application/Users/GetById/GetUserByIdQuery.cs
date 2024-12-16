using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Users.GetById;

public sealed record GetUserByIdQuery(Guid UserId) : IQuery<UserResponse>;