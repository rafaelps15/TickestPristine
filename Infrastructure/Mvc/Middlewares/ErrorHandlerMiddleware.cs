using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using Tickest.Domain.Exceptions;

namespace Tickest.Infrastructure.Mvc.Middlewares;

/// <summary>
/// Middleware para tratamento de erros que intercepta exceções lançadas durante a execução da solicitação HTTP.
/// </summary>
public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    /// <summary>
    /// Inicializa uma nova instância do <see cref="ErrorHandlerMiddleware"/> com os parâmetros fornecidos.
    /// </summary>
    /// <param name="next">Delegado para o próximo middleware na cadeia de processamento de solicitações.</param>
    /// <param name="logger">Instância do logger para registrar erros.</param>
    /// <exception cref="ArgumentNullException">Lançado se <paramref name="next"/> ou <paramref name="logger"/> for <c>null</c>.</exception>
    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Método que é chamado para processar uma solicitação HTTP.
    /// Captura exceções não tratadas, registra o erro e gera uma resposta de erro.
    /// </summary>
    /// <param name="context">O contexto da solicitação HTTP.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Um erro inesperado ocorreu");

            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Gera uma resposta JSON com informações sobre o erro ocorrido.
    /// </summary>
    /// <param name="context">O contexto da solicitação HTTP.</param>
    /// <param name="exception">A exceção que ocorreu.</param>
    /// <returns>Uma tarefa que representa a operação assíncrona de escrita da resposta.</returns>
    /// <exception cref="ArgumentNullException">Lançado se <paramref name="context"/> ou <paramref name="exception"/> for <c>null</c>.</exception>
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        if (context == null)
            throw new ArgumentNullException(nameof(context));

        if (exception == null)
            throw new ArgumentNullException(nameof(exception));

        // Determina o status e a mensagem com base na exceção
        var (statusCode, errorMessage) = exception switch
        {
            ValidationException validationException => ((int)HttpStatusCode.BadRequest, validationException.Message),
            TickestException tickestException => ((int)HttpStatusCode.BadRequest, tickestException.Message),
            UnauthorizedAccessException => ((int)HttpStatusCode.Unauthorized, "Usuário não autorizado"),
            _ => ((int)HttpStatusCode.InternalServerError, "Ocorreu um erro ao processar sua solicitação.")
        };

        // Cria a resposta de erro
        var response = context.Response;
        response.ContentType = "application/json";
        response.StatusCode = statusCode;

        var errorResponse = new ErrorResponse
        {
            Message = errorMessage
        };

        var errorJson = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        });

        return response.WriteAsync(errorJson);
    }
}

/// <summary>
/// Representa a estrutura da resposta de erro enviada ao cliente.
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Mensagem geral sobre o erro.
    /// </summary>
    public string? Message { get; set; }
}
