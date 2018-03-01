

using Microsoft.AspNetCore.Builder;

namespace SensorService.API.Middleware
{
    public static class MiddlewearExtensions
    {
        public static IApplicationBuilder UseAccountService(this IApplicationBuilder app)
        {
            return app.UseMiddleware<AccountServiceMiddleware>();
        }
    }
}