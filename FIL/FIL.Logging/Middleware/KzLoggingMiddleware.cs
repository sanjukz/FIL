using FIL.Logging.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace FIL.Logging.Middleware
{
    public class KzLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public KzLoggingMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                try
                {
                    _logger.Log(LogCategory.Fatal, ex);
                }
                catch (Exception logEx)
                {
                    throw new Exception($"Logging failed, {logEx}", ex);
                }
                throw;
            }
        }
    }
}