using Serilog;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;

    public LoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // כתיבת לוג לפני הטיפול בבקשה
        //Log.Information("Incoming request: {Method} {Path}", context.Request.Method, context.Request.Path);

        await _next(context);

        // כתיבת לוג אחרי הטיפול בבקשה
        //Log.Information("Outgoing response: {StatusCode}", context.Response.StatusCode);
    }
}
