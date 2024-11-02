using Microsoft.Identity.Client;

namespace Social_Media.Middlewares
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;

        // the key name (must be used in the header)
        private const string APIKEY = "XApiKey";
        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /*
            This middleware:
            - check the request header for XApiKey and check for it's value with key in appSettings.
        */
        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(APIKEY, out
                    var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Api Key was not provided ");
                return;
            }
            var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();

            var apiKey = appSettings.GetValue<string>(APIKEY);
            if (!apiKey.Equals(extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized client");
                return;
            }

            await _next(context);
        }
    }

    // add ApiKeyMiddleware to the builder
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseAPIKeyMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiKeyMiddleware>();
        }
    }


}
