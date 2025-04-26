using System.ComponentModel.DataAnnotations;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            _logger.LogError(ex, "Unhandled exception occurred while processing the request.");

            var env = context.RequestServices.GetRequiredService<IWebHostEnvironment>();

            context.Response.ContentType = "application/json";

            var response = new 
            { 
                error = env.IsDevelopment() ? ex.Message : "An unexpected error occurred.", 
                stackTrace = env.IsDevelopment() ? ex.StackTrace : null 
            };

            context.Response.StatusCode = ex switch
            {
                ValidationException => StatusCodes.Status400BadRequest,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };



            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
