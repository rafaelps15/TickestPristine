using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Tickest.Domain.Exceptions;

namespace Tickest.Infrastructure.Mvc.Middlewares;

/// <summary>
/// Middleware global para interceptação e tratamento de erros durante o pipeline de solicitações HTTP.
/// Oferece um mecanismo centralizado para captura, registro e resposta de erros.
/// </summary>
public sealed class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    /// <summary>
    /// Inicializa uma instância de <see cref="ErrorHandlerMiddleware"/>.
    /// </summary>
    /// <param name="next">Delegado para o próximo middleware no pipeline.</param>
    /// <param name="logger">Logger para registrar exceções e informações relevantes.</param>
    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Processa a solicitação HTTP e intercepta exceções não tratadas.
    /// </summary>
    /// <param name="context">Contexto HTTP atual.</param>
    /// <returns>Uma <see cref="Task"/> representando a operação assíncrona.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex) when (LogException(ex)) // Loga e continua o fluxo
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Lógica para gerar uma resposta baseada na exceção capturada.
    /// </summary>
    /// <param name="context">Contexto HTTP atual.</param>
    /// <param name="exception">A exceção capturada.</param>
    /// <returns>Uma <see cref="Task"/> representando a operação assíncrona de resposta.</returns>
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        if (context == null || exception == null)
        {
            throw new ArgumentNullException(context is null ? nameof(context) : nameof(exception));
        }

        // Mapeamento de exceções para códigos e mensagens HTTP
        var (statusCode, message) = exception switch
        {
            ValidationException e => ((int)HttpStatusCode.BadRequest, e.Message),
            TickestException e => ((int)HttpStatusCode.BadRequest, e.Message),
            UnauthorizedAccessException => ((int)HttpStatusCode.Unauthorized, "Usuário não autorizado."),
            _ => ((int)HttpStatusCode.InternalServerError, "Erro interno no servidor.")
        };

        // Configura a resposta HTTP
        var response = context.Response;
        response.ContentType = "application/json";
        response.StatusCode = statusCode;

        // Serializa a resposta de erro
        var errorResponse = new ErrorResponse
        {
            Message = message,
            Timestamp = DateTime.UtcNow
        };

        return response.WriteAsJsonAsync(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });
    }

    /// <summary>
    /// Loga a exceção capturada.
    /// </summary>
    /// <param name="exception">Exceção a ser registrada.</param>
    /// <returns>Sempre retorna <c>true</c> para permitir o fluxo de exceções.</returns>
    private bool LogException(Exception exception)
    {
        _logger.LogError(exception, $"Erro capturado no middleware: {exception.Message}");
        return true; // Permite que o fluxo continue após o log
    }
}

/// <summary>
/// Estrutura para representar detalhes do erro na resposta HTTP.
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Mensagem detalhada sobre o erro ocorrido.
    /// </summary>
    public required string Message { get; init; }

    /// <summary>
    /// Timestamp do momento em que o erro foi gerado.
    /// </summary>
    public required DateTime Timestamp { get; init; }
}
