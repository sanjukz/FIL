using FIL.Configuration;
using FIL.Configuration.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Net.Http;
using System.Text;

namespace FIL.Api.Controllers
{
    public class HealthCheckController : Controller
    {
        private readonly ISettings _settings;

        public HealthCheckController(ISettings settings)
        {
            _settings = settings;
        }

        [Route("health-check")]
        public HttpResponseMessage Get()
        {
            try
            {
                if (System.IO.File.Exists(_settings.GetConfigSetting<string>(SettingKeys.Api.HealthCheckFailFileName)))
                {
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent("Intentional Health Check Failure for Deployment. Remove FailHealthCheck.txt to recover")
                    };
                }
            }
            // Catch both the deliberately thrown exception as well as any internal settings error.
            catch (Exception)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Could not reach configuration service")
                };
            }

            return new HttpResponseMessage
            {
                Content = new StringContent("OK", Encoding.UTF8)
            };
        }
    }
}