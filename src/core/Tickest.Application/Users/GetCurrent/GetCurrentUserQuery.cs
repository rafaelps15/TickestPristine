using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Users.GetCurrent;

public sealed record GetCurrentUserQuery : IQuery<GetCurrentUserResponse>;
