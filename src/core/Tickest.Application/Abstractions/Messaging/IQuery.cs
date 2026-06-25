using MediatR;
using Tickest.SharedKernel;

namespace Tickest.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;

