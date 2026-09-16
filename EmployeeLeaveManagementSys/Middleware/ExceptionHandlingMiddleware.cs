using Microsoft.AspNetCore.Http;

namespace EmployeeLeaveManagementSys.Middleware
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
                    "An unhandled exception occurred.");

                if (!context.Response.HasStarted)
                {
                    context.Response.Clear();

                    context.Response.StatusCode = 500;

                    context.Request.Path = "/Home/Error";

                    await _next(context);
                }
            }
        }
    }
}