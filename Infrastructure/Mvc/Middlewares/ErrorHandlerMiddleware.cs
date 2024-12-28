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

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex) when (LogException(ex,context))
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private bool LogException(Exception exception, HttpContext context)
    {
        var request = context.Request;
        var logMessage = $"Erro capturado no middleware: {exception.Message}, Request Method: {request.Method}, Request Path: {request.Path}, Request Query: {request.QueryString}";

        if (exception is TickestException || exception is ValidationException)
        {
            _logger.LogWarning(exception, logMessage);
        }
        else
        {
            _logger.LogError(exception, logMessage);
        }
        return true;
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        if (context == null || exception == null)
        {
            throw new TickestException(context is null ? nameof(context) : nameof(exception));
        }

        var correlationId = Guid.NewGuid().ToString();

        var (statusCode, message, detailedMessage) = exception switch
        {
            ValidationException e => ((int)HttpStatusCode.BadRequest, e.Message, e.StackTrace),
            TickestException e => ((int)HttpStatusCode.BadRequest, e.Message, e.StackTrace),
            UnauthorizedAccessException => ((int)HttpStatusCode.Unauthorized, "Usuário não autorizado.", null),
            ArgumentNullException => ((int)HttpStatusCode.BadRequest, "Parâmetro inválido", exception.StackTrace),
            _ => ((int)HttpStatusCode.InternalServerError, "Erro interno no servidor.", exception.StackTrace)
        };

        var response = context.Response;
        response.ContentType = "application/json";
        response.StatusCode = statusCode;

        var errorResponse = CreateErrorResponse(message, detailedMessage, correlationId, statusCode);

        return response.WriteAsJsonAsync(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });
    }

    private static ErrorResponse CreateErrorResponse(string message, string? detailedMessage, string correlationId, int statusCode)
    {
        return new ErrorResponse(
            Code: "ERR_" + statusCode,
            Message: message,
            DetailedMessage: detailedMessage,
            CorrelationId: correlationId,
            Timestamp: DateTime.UtcNow
            );
    }
}
