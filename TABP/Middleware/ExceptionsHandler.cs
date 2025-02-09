using Application.Exceptions;
using Application.Exceptions.ExceptionTypes;
using Microsoft.AspNetCore.Diagnostics;

namespace TABP.Middleware
{
    public class ExceptionMiddleware(ILogger<ExceptionMiddleware> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            LogExceptions(exception);
            var response = MapExceptions(exception);
            await Results.Problem(response.Detail, title: response.Title, statusCode: response.StatusCode)
                .ExecuteAsync(httpContext);

            return true;
        }
        private MapExceptionResponse MapExceptions(Exception exception)
        {
            var response = new MapExceptionResponse();
            if (exception is not ExceptionsBase exceptionsBase)
            {
                response.StatusCode = StatusCodes.Status500InternalServerError;
                response.Detail = "An error occurred while processing your request.";
                response.Title = "Internal Server Error";
            }
            else
            {
                response.StatusCode = exceptionsBase switch
                {
                    NotFoundExceptions => StatusCodes.Status404NotFound,
                    BadRequestException => StatusCodes.Status400BadRequest,
                    UnauthorizedException => StatusCodes.Status401Unauthorized,
                    ForbiddenException => StatusCodes.Status403Forbidden,
                    ConflictExceptions => StatusCodes.Status409Conflict,
                    _ => StatusCodes.Status500InternalServerError
                };
                response.Detail = exceptionsBase.Message;
                response.Title = exceptionsBase.Header;
            }
            return response;
        }
        private void LogExceptions(Exception exception)
        {
            if (exception is ExceptionsBase)
            {
                logger.LogWarning(exception, exception.Message);
            }
            else
            {
                logger.LogError(exception, exception.Message);
            }
        }
    }
}