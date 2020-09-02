using FIL.Logging.Middleware;
using Microsoft.AspNetCore.Builder;
using System;

namespace FIL.Logging.Extensions
{
    public static class LoggingExtensions
    {
        public static IApplicationBuilder UseKzLogging(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<KzLoggingMiddleware>();
        }
    }
}