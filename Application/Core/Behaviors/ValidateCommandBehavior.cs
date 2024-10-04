using MediatR;
using Tickest.Application.Interfaces;
using Tickest.Domain.Contracts.Responses;

namespace Tickest.Application.Core.Behaviors;

public class ValidateCommandBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>, ICommandValidator
    where TResponse : IResponse
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        request.Validate();

        return await next();
    }
}
