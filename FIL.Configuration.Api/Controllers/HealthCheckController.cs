using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
using System.Text;

namespace FIL.Configuration.Api.Controllers
{
    public class HealthCheckController : Controller
    {
        [Route("health-check")]
        public HttpResponseMessage HealthCheck()
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("OK", Encoding.UTF8)
            };
        }
    }
}