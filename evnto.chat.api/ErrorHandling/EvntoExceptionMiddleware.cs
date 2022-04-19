using System.Net;

namespace evnto.chat.api.ErrorHandling
{
    public class EvntoExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<EvntoExceptionMiddleware> _logger;

        public EvntoExceptionMiddleware(RequestDelegate next, ILogger<EvntoExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(new ErrorDetails() 
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message
            }.ToString());
        }
    }
}
