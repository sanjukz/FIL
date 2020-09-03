using System.Net.Http;
using FIL.Web.Core.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace FIL.Web.Admin.Controllers
{
    public class HealthCheckController : BaseHealthCheckController
    {
        [Route("health-check")]
        public override HttpResponseMessage HealthCheck()
        {
            return base.HealthCheck();
        }
    }
}
