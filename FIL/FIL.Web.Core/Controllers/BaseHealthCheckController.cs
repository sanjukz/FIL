using System.Net;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace FIL.Web.Core.Controllers
{
    public abstract class BaseHealthCheckController : Controller
    {
        public virtual HttpResponseMessage HealthCheck()
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("OK", Encoding.UTF8)
            };
        }
    }
}
