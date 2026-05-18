using Serilog.Context;

namespace Server_Side_Code_Aol_SoftEng.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {

            var username = context.User.Identity?.Name ?? context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

            using (LogContext.PushProperty("Username", username))
            {
                await _next(context);
            }
        }
    }
}
