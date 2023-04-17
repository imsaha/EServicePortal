namespace EServicePortal.Application.Common.Wrappers;

public class Response
{
    public Response(bool succeeded, string? message = default)
    {
        Succeeded = succeeded;
        Message = message;
    }

    public Response(Dictionary<string, string> errors, bool succeeded, string? message) : this(succeeded, message)
    {
        Errors = errors;
    }

    public bool Succeeded { get; }
    public string? Message { get; }
    public Dictionary<string, string> Errors { get; } = new Dictionary<string, string>();
}

public sealed class Response<T> : Response
{
    public Response(T? data, bool succeeded, string? message = default) : base(succeeded, message)
    {
        Data = data;
    }

    public T? Data { get; }
}

public sealed class PagedResponse<T> : Response
{
    public PagedResponse(bool succeeded, string? message = default) : base(succeeded, message)
    {
        Data = new List<T>();
    }
    public PagedResponse(IEnumerable<T> data, bool succeeded, string? message = default) : this(succeeded, message)
    {
        Data = data;
    }

    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public long TotalCount { get; set; }

    public IEnumerable<T> Data { get; set; }


    /// <summary>
    ///     Map out the paged result
    /// </summary>
    /// <typeparam name="TOut"></typeparam>
    /// <param name="func"></param>
    /// <returns></returns>
    public PagedResponse<TOut> Map<TOut>(Func<PagedResponse<T>, List<TOut>> func) where TOut : notnull
    {
        return new PagedResponse<TOut>(func(this), Succeeded, Message)
        {
            Data = func(this),
            TotalCount = TotalCount,
            PageIndex = PageIndex,
            PageSize = PageSize
        };
    }
}
