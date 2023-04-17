using Microsoft.EntityFrameworkCore;

namespace EServicePortal.Application.Common.Wrappers;

public static class Result
{
    public static Response Fail(string? error = default)
    {
        return new Response(false, error);
    }

    public static Response Assert(bool success, string? message = default)
    {
        return new Response(success, !success && string.IsNullOrEmpty(message) ? "Assertion failed" : message);
    }

    public static Response Success(string? message = default)
    {
        return new Response(true, message);
    }

    public static Response Fail(Dictionary<string, string> failures, string? message = default)
    {
        return new Response(failures, false, message);
    }

    public static Response<T> Fail<T>(string? message = default)
    {
        return new Response<T>(default, false, message);
    }

    public static Response<T> Success<T>(T data, string? message = default)
    {
        return new Response<T>(data, true, message);
    }

    /// <summary>
    ///     returns paged result of <see cref="PagedResponse{T}" />
    /// </summary>
    /// <param name="source">query source</param>
    /// <param name="query">query of type <see cref="IPagedQuery{TOut}" /></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T">return type</typeparam>
    /// <returns></returns>
    static internal Task<PagedResponse<T>> PagedAsync<T>(IOrderedQueryable<T> source, IPagedQuery<T> query, CancellationToken cancellationToken = default)
    {
        return PagedAsync(source, query.PageIndex, query.PageSize ?? 100, cancellationToken);
    }

    private static async Task<PagedResponse<T>> PagedAsync<T>(IOrderedQueryable<T> source, int? pageIndex = default, int? pageSize = default, CancellationToken cancellationToken = default)
    {
        var pIndex = pageIndex > 1 ? pageIndex.Value : 0;
        var pSize = pageSize ?? -1;
        var count = await source.CountAsync(cancellationToken);

        if (pSize == -1)
        {
            pSize = count;
        }
        else if (pageSize > 0 && pageSize < 100)
        {
            pSize = pageSize.Value;
        }
        else
        {
            pSize = 100;
        }

        if (count == 0)
        {
            return new PagedResponse<T>(true)
            {
                TotalCount = count,
                PageIndex = pIndex > 1 ? pIndex : 1,
                PageSize = pSize
            };
        }

        var items = await source.Skip(pIndex * pSize).Take(pSize).ToListAsync(cancellationToken);
        return new PagedResponse<T>(true)
        {
            Data = items,
            TotalCount = count,
            PageIndex = pIndex > 1 ? pIndex : 1,
            PageSize = pSize
        };
    }
}
