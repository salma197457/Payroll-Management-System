using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Payroll.Application.Exceptions;

namespace Payroll.Api.ExceptionHandling
{
    public sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger)
    : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var (statusCode, title, detail) = exception switch
            {
                DbUpdateConcurrencyException => 
                (StatusCodes.Status409Conflict, "Concurrency Conflict", "The record was modified or deleted by another request."),

                DbUpdateException { InnerException: SqlException sqlException } when sqlException.Number is 2601 or 2627 =>
                    (StatusCodes.Status409Conflict, "Conflict", "A record with the same unique value already exists."),

                ConflictException =>
                    (StatusCodes.Status409Conflict, "Conflict", exception.Message),


                InvalidOperationException =>
                    (StatusCodes.Status409Conflict, "Invalid Operation", exception.Message),


                KeyNotFoundException =>
                    (StatusCodes.Status404NotFound, "Not Found", exception.Message),

                ArgumentException =>
                    (StatusCodes.Status400BadRequest, "Bad Request", exception.Message),

                _ =>
                    (StatusCodes.Status500InternalServerError, "Internal Server Error", "An unexpected error occurred.")
            };

            if (statusCode >= 500)
            {
                logger.LogError(exception,
                    "Unhandled exception at {Path}",
                    httpContext.Request.Path);
            }

            var problem = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Instance = httpContext.Request.Path
            };

            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsJsonAsync(
                problem,
                cancellationToken);

            return true;
        }
    }
}
