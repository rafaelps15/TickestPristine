using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Tickest.Domain.Exceptions;
using Tickest.Infrastructure.Mvc.Responses;

namespace Tickest.Infrastructure.Mvc.Middlewares;

public sealed class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex) when (LogException(ex, context))
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private bool LogException(Exception exception, HttpContext context)
    {
        var request = context.Request;

        _logger.Log(
            exception is TickestException or ValidationException ? LogLevel.Warning : LogLevel.Error,
            exception,
            "Erro capturado no middleware: {Message}. Método: {Method}, Caminho: {Path}, Query: {Query}",
            exception.Message,
            request.Method,
            request.Path,
            request.QueryString
        );

        return true; // sempre intercepta
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var correlationId = Guid.NewGuid().ToString();

        var (statusCode, message) = exception switch
        {
            ValidationException ve => ((int)HttpStatusCode.BadRequest, ve.Message),
            TickestException te => ((int)HttpStatusCode.BadRequest, te.Message),
            UnauthorizedAccessException => ((int)HttpStatusCode.Unauthorized, "Usuário não autorizado."),
            ArgumentNullException => ((int)HttpStatusCode.BadRequest, "Parâmetro inválido."),
            _ => ((int)HttpStatusCode.InternalServerError, "Erro interno no servidor.")
        };

        var errorResponse = new ErrorResponse(
            Code: $"ERR_{statusCode}",
            Message: message,
            DetailedMessage: exception.Message,  // Apenas mensagem, sem stacktrace
            CorrelationId: correlationId,
            Timestamp: DateTime.UtcNow
        );

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse, JsonOptions));
    }
}
