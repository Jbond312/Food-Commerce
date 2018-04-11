using Microsoft.AspNetCore.Builder;

namespace Foods.Service.Api.Middleware.Extensions
{
    public static class RateLimitingMiddlewareExtension
    {
        public static IApplicationBuilder UseRateLimitingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RateLimitingMiddleware>();
        }
    }
}