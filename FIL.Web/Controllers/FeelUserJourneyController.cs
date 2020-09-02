using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FIL.Foundation.Senders;
using FIL.Web.Feel.ViewModels.FeelUserJourney;
using FIL.Contracts.Queries.FeelUserJourney;
using FIL.Web.Feel.Modules.SiteExtensions;
using System;
using FIL.Logging.Enums;
using Microsoft.Extensions.Caching.Memory;

namespace FIL.Web.Feel.Controllers
{
    public class FeelUserJourneyController : Controller
    {
        private readonly IQuerySender _querySender;
        private readonly IGeoCurrency _GeoCurrency;
        private readonly IMemoryCache _memoryCache;
        private readonly FIL.Logging.ILogger _logger;

        public FeelUserJourneyController(
            IQuerySender querySender,
            IGeoCurrency GeoCurrency,
            IMemoryCache memoryCache,
             FIL.Logging.ILogger logger
            )
        {
            _querySender = querySender;
            _GeoCurrency = GeoCurrency;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        [HttpPost]
        [Route("api/dynamicsections")]
        public async Task<FeelUserJourneyViewModel> Get([FromBody]FeelUserJourneyRequestViewModel feelUserJourneyRequestViewModel)
        {
            if (feelUserJourneyRequestViewModel == null)
            {
                return new FeelUserJourneyViewModel { };
            }
            string TargetCurrencyCode = _GeoCurrency.GetSessionCurrency(HttpContext);
            if (!_memoryCache.TryGetValue($"{TargetCurrencyCode}_all_places_{feelUserJourneyRequestViewModel.Category}_" +
                $"{feelUserJourneyRequestViewModel.Country}_" +
                $"{feelUserJourneyRequestViewModel.State}_" +
                $"{feelUserJourneyRequestViewModel.City}_" +
                $"{feelUserJourneyRequestViewModel.SubCategory}_" +
                $"{feelUserJourneyRequestViewModel.PageType}_" +
                $"{feelUserJourneyRequestViewModel.PagePath}", out FIL.Contracts.QueryResults.FeelUserJourney.FeelUserJourneyQueryResult result))
            {
                result = await _querySender.Send(new FeelUserJourneyQuery
                {
                    CategoryId = feelUserJourneyRequestViewModel.Category,
                    CountryId = feelUserJourneyRequestViewModel.Country,
                    StateId = feelUserJourneyRequestViewModel.State,
                    CityId = feelUserJourneyRequestViewModel.City,
                    SubCategoryId = feelUserJourneyRequestViewModel.SubCategory,
                    PageType = feelUserJourneyRequestViewModel.PageType,
                    PagePath = feelUserJourneyRequestViewModel.PagePath,
                    MasterEventType = feelUserJourneyRequestViewModel.MasterEventType
                });
                if (result.AllPlaceTiles == null)
                {
                    return new FeelUserJourneyViewModel { };
                }
                try
                {
                    _GeoCurrency.UpdatePlaceDetails(result.AllPlaceTiles.PlaceDetails, HttpContext);
                    foreach (FIL.Contracts.Models.DynamicPlaceSections dynamicSection in result.DynamicPlaceSections)
                    {
                        _GeoCurrency.UpdatePlaceDetails(dynamicSection.PlaceDetails, HttpContext);
                    }
                    _memoryCache.Set($"{TargetCurrencyCode}_all_places_{feelUserJourneyRequestViewModel.Category}_" +
                $"{feelUserJourneyRequestViewModel.Country}_" +
                $"{feelUserJourneyRequestViewModel.State}_" +
                $"{feelUserJourneyRequestViewModel.City}_" +
                $"{feelUserJourneyRequestViewModel.SubCategory}_" +
                $"{feelUserJourneyRequestViewModel.PageType}_" +
                $"{feelUserJourneyRequestViewModel.PagePath}", result, DateTime.Now.AddMinutes(10));

                    return new FeelUserJourneyViewModel
                    {
                        GeoData = result.GeoData,
                        SearchValue = result.SearchValue,
                        ContryPageDetails = result.ContryPageDetails,
                        AllPlaceTiles = result.AllPlaceTiles,
                        DynamicPlaceSections = result.DynamicPlaceSections,
                        SubCategories = result.SubCategories,
                        Success = result.Success
                    };
                }
                catch (Exception e)
                {
                    _logger.Log(LogCategory.Error, e);
                    return new FeelUserJourneyViewModel
                    {
                        GeoData = result.GeoData,
                        SearchValue = result.SearchValue,
                        ContryPageDetails = result.ContryPageDetails,
                        AllPlaceTiles = result.AllPlaceTiles,
                        DynamicPlaceSections = result.DynamicPlaceSections,
                        SubCategories = result.SubCategories,
                        Success = result.Success
                    };
                }
            }
            _GeoCurrency.UpdatePlaceDetails(result.AllPlaceTiles.PlaceDetails, HttpContext);
            foreach (FIL.Contracts.Models.DynamicPlaceSections dynamicSection in result.DynamicPlaceSections)
            {
                _GeoCurrency.UpdatePlaceDetails(dynamicSection.PlaceDetails, HttpContext);
            }
            return new FeelUserJourneyViewModel
            {
                GeoData = result.GeoData,
                SearchValue = result.SearchValue,
                ContryPageDetails = result.ContryPageDetails,
                AllPlaceTiles = result.AllPlaceTiles,
                DynamicPlaceSections = result.DynamicPlaceSections,
                SubCategories = result.SubCategories,
                Success = result.Success
            };
        }
    }
}
