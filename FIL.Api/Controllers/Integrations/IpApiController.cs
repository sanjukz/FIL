using FIL.Api.Integrations;
using FIL.Contracts.Models.Integrations;
using Microsoft.AspNetCore.Mvc;

namespace FIL.Api.Controllers
{
    public class IpApiController : Controller
    {
        private readonly IIpApi _ipApi;

        public IpApiController(IIpApi ipApi)
        {
            _ipApi = ipApi;
        }

        [HttpGet]
        [Route("api/ip/{ip}")]
        public IpApiResponse GetSession(string ip)
        {
            IResponse<IpApiResponse> ipApiResponse = _ipApi.GetUserLocationByIp(ip).Result;
            return ipApiResponse.Result;
        }
    }
}