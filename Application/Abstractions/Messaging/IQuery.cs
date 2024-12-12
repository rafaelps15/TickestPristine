using MediatR;
using Tickest.Domain.Common;

namespace Tickest.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;

