using FluentValidation;
using MediatR;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Exceptions;

namespace Tickest.Application.Abstractions.Behaviors;

internal sealed class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
    where TResponse : class
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is IQuery<TResponse>)
            return await next();

        var context = new ValidationContext<TRequest>(request);

        var failures = (await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken))
        ))
        .SelectMany(result => result.Errors)
        .Where(f => f != null)
        .ToList();

        var errors = failures
            .Distinct()
            .Select(p => p.ErrorMessage)
            .ToList();

        if (failures.Count != 0)
            throw new TickestException(string.Join(Environment.NewLine, errors));

        return await next();
    }
}
