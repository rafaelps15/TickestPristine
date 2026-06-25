using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Tickest.SharedKernel;
using Tickest.SharedKernel.Exceptions;

namespace WebApi.Infrastructure;

internal static class ApiProblemDetailsFactory
{
    private const string BadRequestType = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
    private const string UnauthorizedType = "https://tools.ietf.org/html/rfc7235#section-3.1";
    private const string NotFoundType = "https://tools.ietf.org/html/rfc7231#section-6.5.4";
    private const string ConflictType = "https://tools.ietf.org/html/rfc7231#section-6.5.8";
    private const string InternalServerErrorType = "https://tools.ietf.org/html/rfc7231#section-6.6.1";

    public static ProblemDetails Create(Exception exception, HttpContext httpContext, DateTime utcNow)
    {
        var statusCode = GetStatusCode(exception);

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Type = GetType(statusCode),
            Title = GetTitle(exception),
            Detail = exception.Message,
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;
        problemDetails.Extensions["timestamp"] = utcNow;

        if (exception is ValidationException validationException)
        {
            problemDetails.Extensions["errors"] = validationException.Errors
                .Select(error => error.ErrorMessage)
                .ToArray();
        }

        return problemDetails;
    }

    public static ProblemDetails Create(Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("Não é possível criar ProblemDetails para um resultado de sucesso.");
        }

        var statusCode = GetStatusCode(result.Error.Type);

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Type = GetType(statusCode),
            Title = result.Error.Code,
            Detail = result.Error.Description
        };

        if (result.Error is ValidationError validationError)
        {
            problemDetails.Extensions["errors"] = validationError.Errors;
        }

        return problemDetails;
    }

    private static int GetStatusCode(Exception exception) =>
        exception switch
        {
            TickestException => StatusCodes.Status400BadRequest,
            ValidationException => StatusCodes.Status400BadRequest,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };

    private static int GetStatusCode(ErrorType errorType) =>
        errorType switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Problem => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

    private static string GetTitle(Exception exception) =>
        exception switch
        {
            TickestException => "Erro de regra de negócio.",
            ValidationException => "Erro de validação.",
            UnauthorizedAccessException => "Usuário não autorizado.",
            _ => "Erro interno no servidor."
        };

    private static string GetType(int statusCode) =>
        statusCode switch
        {
            StatusCodes.Status400BadRequest => BadRequestType,
            StatusCodes.Status401Unauthorized => UnauthorizedType,
            StatusCodes.Status404NotFound => NotFoundType,
            StatusCodes.Status409Conflict => ConflictType,
            _ => InternalServerErrorType
        };
}
