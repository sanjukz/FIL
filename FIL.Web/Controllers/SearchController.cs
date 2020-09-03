using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FIL.Web.Feel.ViewModels.Search;
using FIL.Contracts.Queries.CitySearch;
using FIL.Foundation.Senders;
using FIL.Contracts.Queries.State;
using FIL.Contracts.Queries.FeelSearch;
using FIL.Contracts.Enums;
using FIL.Logging.Enums;
using FIL.Utilities.Extensions;
using FIL.Web.Core.Providers;
using FIL.Web.Feel.Builders;
using FIL.Web.Feel.Providers;

namespace FIL.Web.Feel.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearchProvider _searchProvider;
        private readonly IQuerySender _querySender;
        private readonly ISiteIdProvider _siteIdProvider;
        private readonly IEventSearchResultBuilder _eventSearchResultBuilder;
        private readonly Logging.ILogger _logger;

        public SearchController(IQuerySender querySender,
            ISiteIdProvider siteIdProvider,
            ISearchProvider searchProvider,
            IEventSearchResultBuilder eventSearchResultBuilder,
            FIL.Logging.ILogger logger)
        {
            _searchProvider = searchProvider;
            _querySender = querySender;
            _siteIdProvider = siteIdProvider;
            _eventSearchResultBuilder = eventSearchResultBuilder;
            _logger = logger;
        }

        [HttpGet]
        [Route("api/search/{searchString}")]
        public async Task<SearchResponseViewModel> GetByName(string searchString, SiteLevel? siteLevel = null)
        {
            if (searchString.IsNullOrBlank() || searchString.Trim().Length < 2)
            {
                return new SearchResponseViewModel();
            }

            // TODO: XXX: NSP: aggregate both searches into parallel queries
            var siteId = _siteIdProvider.GetSiteId();
            siteLevel = siteLevel ?? SiteLevel.Global;
            // TODO: XXX: NSP: Search / Site level caching in Redis (not local) - can also derive all locations from IA
            var queryResult = await _querySender.Send(new FeelSearchQuery
            {
                Name = searchString,
                SiteId = siteId,
                SiteLevel = siteLevel.Value
            });
            IList<EventSearchResult> searchResults = new List<EventSearchResult>();
            try
            {
                //searchResults = await _searchProvider.Search(searchString, siteId, siteLevel.Value, false);
                if (!searchResults.Any())
                {
                    _logger.Log(LogCategory.Warn, "No IA results returned", new Dictionary<string, object>
                    {
                        ["SearchString"] = searchString
                    });
                    //var categoryEventQueryResult = await _querySender.Send(new CategoryEventSearchQuery
                    //{
                    //    Name = searchString,
                    //    SiteId = siteId
                    //});

                    //searchResults = categoryEventQueryResult
                    //       .CategoryEvents?
                    //       .Select(_eventSearchResultBuilder.Build)
                    //       .ToList() ?? new List<EventSearchResult>();
                     searchResults = new List<EventSearchResult>();

                }
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, e);
            }
            return new SearchResponseViewModel
            {
                CategoryEvents = searchResults.ToList(),
                Cities = queryResult.Cities,
                States = queryResult.States,
                Countries = queryResult.Countries,
            };
        }

        [Route("api/search/{searchString}/{cityName}/{countryName}/{category}/{subCategory}")]
        public async Task<SearchResponseViewModel> GetSearchResult(string searchString, string cityName, string countryName, string category, string subCategory, SiteLevel? siteLevel = null)
        {
            if (searchString.IsNullOrBlank() || searchString.Trim().Length < 2)
            {
                return new SearchResponseViewModel();
            }

            // TODO: XXX: NSP: aggregate both searches into parallel queries
            var siteId = _siteIdProvider.GetSiteId();
            siteLevel = siteLevel ?? SiteLevel.Global;
            // TODO: XXX: NSP: Search / Site level caching in Redis (not local) - can also derive all locations from IA
            var queryResult = await _querySender.Send(new FeelSearchQuery
            {
                Name = searchString,
                SiteId = siteId,
                SiteLevel = siteLevel.Value
            });

            //var searchResults = await _searchProvider.Search(searchString, siteId, siteLevel.Value);
            //if (!searchResults.Any())
            //{
            //_logger.Log(LogCategory.Warn, "No IA results returned", new Dictionary<string, object>
            //{
            //    ["SearchString"] = searchString
            //});

            //commenting IA 

            //var categoryEventQueryResult = await _querySender.Send(new CategoryEventSearchQuery
            //{
            //    Name = searchString,
            //    SiteId = siteId,
            //    CityName = cityName,
            //    CountryName = countryName,
            //    Category = category,
            //    SubCategory = subCategory
            //});
            //var searchResults = categoryEventQueryResult
            //        .CategoryEvents?
            //        .Select(_eventSearchResultBuilder.Build)
            //        .ToList() ?? new List<EventSearchResult>();
            //}
            var searchResults = new List<EventSearchResult>();
            return new SearchResponseViewModel
            {
                CategoryEvents = searchResults,
                Cities = queryResult.Cities,
                States = queryResult.States,
                Countries = queryResult.Countries,
            };
        }
    }
}
