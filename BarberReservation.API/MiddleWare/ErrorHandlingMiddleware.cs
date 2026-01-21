using BarberReservation.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace BarberReservation.API.MiddleWare;

public sealed class ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch(Exception ex)
        {
            if(context.Response.HasStarted)
            {
                logger.LogError(ex, "Response already started. TraceId: {Traceid}", context.TraceIdentifier);
                throw;
            }

            ProblemDetails problem = CreateProblemDetails(context, ex, out int statusCode);

            logger.Log(
                GetLogLevel(statusCode),
                ex,
                "Handled exception. StatusCode: {StatusCode}, TraceId: {TraceId}, Path: {Path}, Message: {Message}",
                statusCode,
                context.TraceIdentifier,
                context.Request.Path.Value,
                ex.Message);

            context.Response.Clear();
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/problem+json";

            await context.Response.WriteAsJsonAsync(problem);
        }
    }

    private static ProblemDetails CreateProblemDetails(HttpContext context, Exception ex, out int statusCode)
    {
        (statusCode, string title, string detail) = ex switch
        {
            NotFoundException nf => (StatusCodes.Status404NotFound, "Nenalezeno", nf.Message),
            ConflictException cf => (StatusCodes.Status409Conflict, "Konflikt", cf.Message),
            ForbiddenException fb => (StatusCodes.Status403Forbidden, "Zakázáno", fb.Message),
            UnauthorizedException un => (StatusCodes.Status401Unauthorized, "Neautorizováno", un.Message),
            DomainException de => (StatusCodes.Status400BadRequest, "Chyba domény", de.Message),
            ArgumentException ae => (StatusCodes.Status400BadRequest, "Neplatný vstup", ae.Message),
            ValidationException ve => (StatusCodes.Status400BadRequest, "Neplatný vstup", ve.Message),

            _ => (StatusCodes.Status500InternalServerError, "Chyba serveru", "Došlo k neočekávané chybě na serveru.")
        };

        var pd = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Type = $"https://httpstatuses.io/{statusCode}",
            Instance = context.Request.Path
        };

        pd.Extensions["traceId"] = context.TraceIdentifier;
        return pd;
    }

    private static LogLevel GetLogLevel(int statusCode)
    {
        return statusCode >= 500 ? LogLevel.Error : LogLevel.Warning;
    }
}
