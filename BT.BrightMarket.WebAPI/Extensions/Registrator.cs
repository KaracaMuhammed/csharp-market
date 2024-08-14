using BT.BrightMarket.WebAPI.Middleware;
using Microsoft.AspNetCore.Builder;

namespace BT.BrightMarket.WebAPI.Extensions
{
    public static class Registrator
    {
        public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            return app;
        }

        public static IApplicationBuilder UseAuthorizationMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomAuthorizationMiddleware>();
            return app;
        }
    }
}
