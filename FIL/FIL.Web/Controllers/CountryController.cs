using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FIL.Contracts.Queries.Country;
using FIL.Foundation.Senders;
using FIL.Web.Feel.ViewModels.Country;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using FIL.Web.Core.Providers;
using FIL.Contracts.Enums;

namespace FIL.Web.Feel.Controllers
{
    public class CountryController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IQuerySender _querySender;
        private readonly IMemoryCache _memoryCache;
        private readonly ISiteIdProvider _siteIdProvider;

        public CountryController(ICommandSender commandSender, IQuerySender querySender, IMemoryCache memoryCache, ISiteIdProvider siteIdProvider)
        {
            _commandSender = commandSender;
            _querySender = querySender;
            _memoryCache = memoryCache;
            _siteIdProvider = siteIdProvider;
        }

        [HttpGet]
        [Route("api/country/all")]
        public async Task<CountryResponseViewModel> GetAll()
        {
            var SiteId = _siteIdProvider.GetSiteId();
            if (SiteId == Site.DevelopmentSite)
            {
                SiteId = Site.feelaplaceSite;
            }

            if (!_memoryCache.TryGetValue($"{SiteId}_countries", out List<FIL.Contracts.Models.Country> Countries))
            {
                var queryResult = await _querySender.Send(new CountryQuery());
                if (queryResult.Countries != null)
                {
                    _memoryCache.Set($"{SiteId}_countries", queryResult.Countries, DateTime.Now.AddMinutes(120));
                    return new CountryResponseViewModel { Countries = queryResult.Countries };
                }
                else
                {
                    return null;
                }
            }
            return new CountryResponseViewModel
            {
                Countries = Countries
            };
        }

        [HttpGet]
        [Route("api/country/{altId}")]
        public async Task<CountryResponseViewModel> GetByAltId(Guid altId)
        {
            var queryResult = await _querySender.Send(new CountryQuery { AltId = altId });
            return queryResult.Countries != null ? new CountryResponseViewModel { Countries = queryResult.Countries } : null;
        }
    }
}
