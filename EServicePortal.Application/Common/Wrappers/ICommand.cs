using MediatR;

namespace EServicePortal.Application.Common.Wrappers;

/// <summary>
///     Command action that returns no data
/// </summary>
internal interface ICommand : IRequest<Response>
{
}

/// <summary>
///     Handler for command action that returns no data
/// </summary>
/// <typeparam name="TRequest"></typeparam>
internal interface ICommandHandler<in TRequest> :
    IRequestHandler<TRequest, Response> where TRequest : ICommand
{
}

/// <summary>
///     Command action that returns <see cref="T" />
/// </summary>
/// <typeparam name="T"></typeparam>
internal interface ICommand<T> : IRequest<Response<T>>
{
}

/// <summary>
///     Handler for command action that returns <see cref="TResponse" />
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
internal interface ICommandHandler<in TRequest, TResponse> :
    IRequestHandler<TRequest, Response<TResponse>> where TRequest : ICommand<TResponse>
{
}
