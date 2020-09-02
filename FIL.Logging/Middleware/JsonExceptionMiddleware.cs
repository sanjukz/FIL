using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace FIL.Logging.Middleware
{
    /// <summary>
    /// https://www.recaffeinate.co/post/serialize-errors-as-json-in-aspnetcore/
    /// </summary>
    public class JsonExceptionMiddleware
    {
        public const string DefaultErrorMessage = "A server error occurred.";
        private readonly IHostingEnvironment _env;
        private readonly JsonSerializer _jsonSerializer;

        public JsonExceptionMiddleware(IHostingEnvironment env)
        {
            _env = env;
            _jsonSerializer = new JsonSerializer
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        public async Task Invoke(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;
            if (ex == null) return;

            context.Response.ContentType = "application/json";

            using (var writer = new StreamWriter(context.Response.Body))
            {
                _jsonSerializer.Serialize(writer, BuildError(ex));
                await writer.FlushAsync().ConfigureAwait(false);
            }
        }

        private object BuildError(Exception ex)
        {
            if (_env.IsDevelopment())
            {
                return new
                {
                    ex.Message,
                    Detail = ex.StackTrace
                };
            }

            return new
            {
                Message = DefaultErrorMessage,
                Detail = ex.Message
            };
        }
    }
}