using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using EServicePortal.Application.Common.Wrappers;
using FluentValidation;

namespace EServicePortal.Middlewares;

public class ErrorMiddleware : IMiddleware
{
    private readonly ILogger<ErrorMiddleware> _logger;

    private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
    {
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public ErrorMiddleware(ILogger<ErrorMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {

        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var message = string.IsNullOrEmpty(ex.Message) ? "Error occurred" : ex.Message;
            _logger.LogError(ex, message);

            switch (ex)
            {
                case ValidationException validationException:
                {
                    context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    var failures = validationException.Errors
                        .GroupBy(x => x.PropertyName)
                        .ToDictionary(
                            k => k.Key,
                            v => v.Where(x => !string.IsNullOrEmpty(x.ErrorMessage)).Select(s => s.ErrorMessage).FirstOrDefault() ?? "");

                    await context.Response.WriteAsJsonAsync(Result.Fail(failures, message), _serializerOptions);
                    break;
                }
                default:
                    // TODO: Map exception with valid status code
                    context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    await context.Response.WriteAsJsonAsync(Result.Fail(message), _serializerOptions);
                    break;
            }

        }
    }
}

public static class ErrorMiddlewareExtension
{
    public static void AddGlobalErrorHandler(this IServiceCollection services)
    {
        services.AddTransient<ErrorMiddleware>();
    }
    public static void UseGlobalErrorHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<ErrorMiddleware>();
    }
}
