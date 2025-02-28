using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
  // private readonly RequestDelegate _next = next;
  // private readonly IHostEnvironment _env = env;
  // private readonly ILogger<ExceptionMiddleware> _logger = logger;

  public async Task InvokeAsync(HttpContext context)
  {
    try
    {
      await next(context);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, ex.Message);
      context.Response.ContentType = "application/json";
      context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

      // var response = env.IsDevelopment()
      //   ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
      //   : new ApiException(context.Response.StatusCode, "Internal Server Error", "Internal server error");


      var response = context.Response.StatusCode switch
      {
        (int)HttpStatusCode.NotFound => new ApiException(context.Response.StatusCode, "Resource not found", ex.Message),
        (int)HttpStatusCode.BadRequest => new ApiException(context.Response.StatusCode, "Bad request", ex.Message),
        _ => new ApiException(context.Response.StatusCode, "Internal Server Error", ex.Message)
      };

      var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

      var json = JsonSerializer.Serialize(response, options);

      await context.Response.WriteAsync(json);
    }
  }
}
