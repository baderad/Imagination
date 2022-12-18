using System;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Imagination.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Imagination.Middleware;

internal sealed class ExceptionHandlingMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _log;
    private readonly RequestDelegate _next;
    private readonly IActionResultExecutor<ObjectResult> _objectResultExecutor;

    public ExceptionHandlingMiddleware(RequestDelegate next, IActionResultExecutor<ObjectResult> objectResultExecutor,
        ILogger<ExceptionHandlingMiddleware> log)
    {
        _next = next;
        _log = log;
        _objectResultExecutor = objectResultExecutor;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context).ConfigureAwait(true);
        }
        catch (Exception exception)
        {
            var request = context.Request;

            _log.LogError(exception,
                $"Exception on {request.Method} {HttpUtility.UrlDecode(request.GetEncodedPathAndQuery())}");

            context.Response.StatusCode = (int) MapExceptionToHttpStatusCode(exception);

            await _objectResultExecutor
                .ExecuteAsync(new ActionContext { HttpContext = context }, new ObjectResult(new { exception.Message }))
                .ConfigureAwait(true);
        }
    }

    private static HttpStatusCode MapExceptionToHttpStatusCode(Exception exception)
    {
        return exception switch
        {
            ImageConversionException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };
    }
}

internal static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}