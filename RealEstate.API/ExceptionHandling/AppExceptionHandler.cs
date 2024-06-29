using Microsoft.AspNetCore.Diagnostics;

namespace RealEstate.API.ExceptionHandling
{
    public class AppExceptionHandler(ILogger<AppExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(exception, exception.Message);
            var response = new ErrorResponse()
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = exception.Message,
                Title = "Somethings went wrong"
            };
            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
            return true;
        }
    }
}
