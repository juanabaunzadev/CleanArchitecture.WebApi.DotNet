using System.Net;
using System.Text.Json;
using CleanArchitecture.WebApi.Application.Exceptions;
using FluentValidation;

namespace CleanArchitecture.WebApi.Presentation.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch(Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";
        var result = string.Empty;

        switch (exception)
        {
            case NotFoundException:
                httpStatusCode = HttpStatusCode.NotFound;
                break;
            case ValidationException validationException:
                httpStatusCode = HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(validationException.Errors);
                break;
        }

        context.Response.StatusCode = (int)httpStatusCode;
        return context.Response.WriteAsync(result);
    }   
}

public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}