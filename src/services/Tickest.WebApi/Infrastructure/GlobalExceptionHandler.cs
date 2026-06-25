using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Tickest.SharedKernel;
using Tickest.SharedKernel.Exceptions;

namespace WebApi.Infrastructure;

internal sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IDateTimeProvider dateTimeProvider) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        LogException(exception);

        var problemDetails = ApiProblemDetailsFactory.Create(
            exception,
            httpContext,
            dateTimeProvider.UtcNow);

        httpContext.Response.StatusCode = problemDetails.Status!.Value;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private void LogException(Exception exception)
    {
        if (exception is TickestException or ValidationException)
        {
            logger.LogWarning(exception, "Erro tratado pela aplicação.");
            return;
        }

        logger.LogError(exception, "Erro inesperado na aplicação.");
    }
}
