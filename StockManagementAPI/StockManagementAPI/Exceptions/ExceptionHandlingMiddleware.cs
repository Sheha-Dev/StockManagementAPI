using System.Text.Json;

namespace StockManagementAPI.Exceptions
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
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
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An exception occurred. TraceId: {TraceId}",
                    context.TraceIdentifier);

                if (context.Response.HasStarted)
                {
                    throw;
                }

                context.Response.Clear();

                var statusCode = ex is APIException apiException
                    ? apiException.StatusCode
                    : StatusCodes.Status500InternalServerError;

                var message = ex is APIException knownException
                    ? knownException.Message
                    : "An unexpected error occurred.";

                var errorCode = ex is APIException knownError
                    ? knownError.ErrorCode
                    : "INTERNAL_SERVER_ERROR";

                var response = new ApiErrorResponse
                {
                    StatusCode = statusCode,
                    Message = message,
                    ErrorCode = errorCode,
                    TraceId = context.TraceIdentifier
                };

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(
                    response,
                    new JsonSerializerOptions(
                        JsonSerializerDefaults.Web));
            }
        }
    }
}
