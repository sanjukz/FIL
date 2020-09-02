using Microsoft.AspNetCore.Http;

namespace FIL.Web.Core.Providers
{
    public interface ISiteIdProvider
    {
        FIL.Contracts.Enums.Site GetSiteId();
    }

    public abstract class BaseSiteIdProvider : ISiteIdProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
         
        protected BaseSiteIdProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected string GetHost()
        {
            return _httpContextAccessor.HttpContext.Request.Host.Value;
        }

        public abstract Contracts.Enums.Site GetSiteId();

    }
}