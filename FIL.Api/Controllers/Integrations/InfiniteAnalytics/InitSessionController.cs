using FIL.Api.Integrations.InfiniteAnalytics;
using FIL.Contracts.Models.Integrations;
using FIL.Contracts.Models.Integrations.InfiniteAnalytics;
using Microsoft.AspNetCore.Mvc;

namespace FIL.Api.Controllers
{
    public class InitSessionController : Controller
    {
        private readonly IInitSession _initSession;

        public InitSessionController(IInitSession initSession)
        {
            _initSession = initSession;
        }

        [HttpGet]
        [Route("api/infiniteanalytics/session")]
        public IResponse<SessionResponse> GetSession()
        {
            return _initSession.GetSession().Result;
        }
    }
}