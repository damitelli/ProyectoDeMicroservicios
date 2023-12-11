namespace Application.Middlewares;

/// <Summary>
/// Maneja todas las excepciones del microservicio.
/// <Summary>
public class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILoggerService _logger;

    public ExceptionHandlingMiddleware(ILoggerService logger) => _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            _logger.LogError($"Unexpected error {e}");
            await HandleExceptionAsync(context, e);
        }
    }

    /// <summary>
    /// Obtiene el HTTP status code y el mensaje correspondiente a la excepción.
    /// </summary>
    /// <param name="exception">La excepción.</param>
    /// <returns>Retorna el status code y el mensaje al caller.</returns>
    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var statusCode = GetStatusCode(exception);
        var response = new
        {
            status = statusCode,
            message = exception.Message
        };
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static int GetStatusCode(Exception exception) =>
    exception switch
    {
        BadRequestException => StatusCodes.Status400BadRequest,
        FavoriteItemNotFoundException or
        UserFavoriteListNotFoundException or
        ListByUserIdNotFoundException => StatusCodes.Status404NotFound,
        InvalidOperationException => StatusCodes.Status409Conflict,
        _ => StatusCodes.Status500InternalServerError
    };
}
