using System.Linq;
using Microsoft.AspNetCore.Http;

namespace FIL.Web.Core.Providers
{
    public interface IClientIpProvider
    {
        string Get();
    }

    public class ClientIpProvider : IClientIpProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
         
        public ClientIpProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Get()
        {
            var hasForwardedIps = _httpContextAccessor.HttpContext.Request.Headers.ContainsKey("X-Forwarded-For");
            var isLocalHost = _httpContextAccessor.HttpContext.Request.Host.Value.Contains("localhost");
            if (hasForwardedIps)
            {
                var ipPath = _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"].ToString().Split(',');
                return ipPath.First();
            }
            else if (isLocalHost)
            {
                return "54.202.47.54";
            } else
            {
                return _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            }
        }
    }
}