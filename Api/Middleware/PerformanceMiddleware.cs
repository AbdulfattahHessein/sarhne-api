using System.Diagnostics;

namespace Api.Middleware;

public class PerformanceMiddleware(RequestDelegate next, ILogger<PerformanceMiddleware> logger)
{
    private readonly ILogger<PerformanceMiddleware> _logger = logger;
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        const int performanceTimeLog = 500;

        var sw = new Stopwatch();

        sw.Start();

        await _next(context);

        sw.Stop();

        if (sw.ElapsedMilliseconds > performanceTimeLog)
            _logger.LogWarning(
                "Request {method} {path} it took about {elapsed} ms",
                context.Request?.Method,
                context.Request?.Path.Value,
                sw.ElapsedMilliseconds
            );
    }
}
