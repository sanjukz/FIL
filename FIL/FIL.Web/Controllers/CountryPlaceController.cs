using System;
using System.Threading.Tasks;
using FIL.Contracts.Queries.CountryPlace;
using FIL.Foundation.Senders;
using FIL.Web.Core.Providers;
using FIL.Web.Feel.Modules.SiteExtensions;
using FIL.Web.Feel.ViewModels.CountryPlace;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace FIL.Web.Feel.Controllers
{
    public class CountryPlaceController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IMemoryCache _memoryCache;
        private readonly ISiteIdProvider _siteIdProvider;
        private readonly IQuerySender _querySender;
        private readonly IGeoCurrency _geoCurrency;

        public CountryPlaceController(ICommandSender commandSender, IQuerySender querySender, ISiteIdProvider siteIdProvider, IMemoryCache memoryCache,
            IGeoCurrency geoCurrency)
        {
            _commandSender = commandSender;
            _querySender = querySender;
            _memoryCache = memoryCache;
            _siteIdProvider = siteIdProvider;
            _geoCurrency = geoCurrency;
        }

        [HttpGet]
        [Route("api/get/countryplace/{categoryId?}")]
        public async Task<CountryPlaceViewModel> GetAll(int categoryId)
        {
            if (!_memoryCache.TryGetValue($"all_country", out FIL.Contracts.QueryResults.CountryPlace.CountryPlaceQueryResult result))
            {
                var queryResult = await _querySender.Send(new CountryPlaceQuery
                {
                    ParentCategoryId = categoryId
                });
                result = queryResult;
                _memoryCache.Set($"all_country", queryResult, DateTime.Now.AddMinutes(20));
            }
            return new CountryPlaceViewModel
            {
                CountryCategoryCounts = result.CountryCategoryCounts,
                CountryPlace = result.CountryPlaces
            };
        }
    }
}
