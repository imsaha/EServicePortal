using MediatR;

namespace EServicePortal.Application.Common.Wrappers;

/// <summary>
///     Query action returns <see cref="T" />
/// </summary>
/// <typeparam name="T"></typeparam>
internal interface IQuery<T> : IRequest<Response<T>>
{
}

/// <summary>
///     Query action returns <see cref="TResponse" />
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
internal interface IQueryHandler<in TRequest, TResponse> :
    IRequestHandler<TRequest, Response<TResponse>> where TRequest : IQuery<TResponse>
{
}

/// <summary>
///     Query returns paged result of <see cref="TOut" />
/// </summary>
/// <typeparam name="TOut"></typeparam>
internal interface IPagedQuery<TOut> : IRequest<PagedResponse<TOut>>
{
    int PageIndex { get; set; }
    int? PageSize { get; set; }
}

/// <summary>
///     Handler for query
/// </summary>
/// <typeparam name="TIn"></typeparam>
/// <typeparam name="TOut"></typeparam>
internal interface IPagedQueryHandler<in TIn, TOut> : IRequestHandler<TIn, PagedResponse<TOut>> where TIn : IPagedQuery<TOut>
{
}
