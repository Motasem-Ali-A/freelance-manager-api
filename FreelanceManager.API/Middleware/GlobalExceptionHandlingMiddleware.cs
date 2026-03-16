using FreelanceManager.Core.Exceptions;

namespace FreelanceManager.API.Middleware
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
        public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
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
            catch (NotFoundException e)
            {
                _logger.LogError(e, e.Message);
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("{\"error\": \""+ e.Message + "\"}");

            }
            catch (ValidationException e)
            {
                _logger.LogError(e, e.Message);
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("{\"error\": \""+ e.Message + "\"}");
            }
            catch (Exception e)
            {
                 _logger.LogError(e, e.Message);
                 context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                 context.Response.ContentType = "application/json";
                 await context.Response.WriteAsync("{\"error\": \"An unexpected error occurred\"}");
            }
        }
    }
}