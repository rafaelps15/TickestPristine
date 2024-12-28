using MediatR;

using Tickest.Domain.Common;

namespace Tickest.Application.Abstractions.Messaging;

//public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
//    where TCommand : ICommand<TResponse>
//{
//}

public interface ICommandHandler<in TCommand>
    : IRequestHandler<TCommand, Result>
    where TCommand : ICommand;

public interface ICommandHandler<in TCommand, TResponse>
    : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>;
