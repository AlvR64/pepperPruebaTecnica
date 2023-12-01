using ISF.FAF_RF.Domain.Common;

namespace LibraryPepper.API.Middlewares
{
    public class ErrorsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorsMiddleware> _logger;

        public ErrorsMiddleware(RequestDelegate next, ILogger<ErrorsMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string? body = await GetBody(context);
            try
            {
                await _next(context);
            }
            catch (OperationCanceledException ex)
            {
                var message = $"Operation canceled";
                await GenerateLogAndSendResponseForOperationCanceledExceptions(context, StatusCodes.Status408RequestTimeout, ex, message, body);
            }
            catch (AppException ex)
            {
                var message = $"Application error: {ex.Error.Message}";
                await GenerateLogAndSendResponseForAppExceptions(context, StatusCodes.Status408RequestTimeout, ex, ex.Error, message, body);
            }
            catch (Exception ex)
            {
                var message = $"Unhandle Exception: {ex.Message}";
                await GenerateLogAndSendResponseForAppExceptions(context, StatusCodes.Status500InternalServerError, ex, AppError.UnhandleError, message, body);
            }
        }

        private async Task GenerateLogAndSendResponseForAppExceptions(HttpContext context, int status, Exception ex, AppError error, string message, string? body)
        {
            string? path = context.Request.Path.ToString();
            _logger.LogError(new Exception($" {message}\n request path: {path}\n request body: {body}", ex), "");
            context.Response.StatusCode = status;
            await context.Response.WriteAsJsonAsync(error);
        }

        private async Task GenerateLogAndSendResponseForOperationCanceledExceptions(HttpContext context, int status, Exception ex, string message, string? body)
        {
            string? path = context.Request.Path.ToString();
            _logger.LogError(new Exception($" {message}\n request path: {path}\n request body: {body}", ex), "");
            context.Response.StatusCode = status;
            await context.Response.WriteAsJsonAsync(ex.Message);
        }

        private static async Task<string?> GetBody(HttpContext context)
        {
            string? body = null;
            context.Request.EnableBuffering();
            var stream = context.Request.Body;
            var reader = new StreamReader(stream, null, true, 1024, true);
            try
            {
                body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }
            catch (Exception) { }
            return body;
        }
    }
}
