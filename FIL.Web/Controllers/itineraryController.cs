using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FIL.Foundation.Senders;
using FIL.Web.Core.Providers;
using FIL.Web.Feel.Providers;
using Microsoft.Extensions.Caching.Memory;
using FIL.Web.Feel.ViewModels.Itinerary;
using FIL.Web.Feel.ViewModels.MasterBudgetRange;
using FIL.Contracts.Queries.MasterBudgetRange;
using FIL.Web.Feel.Modules.SiteExtensions;
using FIL.Web.Feel.ViewModels.Category;
using FIL.Contracts.Enums;

namespace FIL.Web.Feel.Controllers
{
    public class ItineraryController : Controller
    {
        private readonly IQuerySender _querySender;
        private readonly ISearchProvider _searchProvider;
        private readonly IMemoryCache _memoryCache;
        private readonly ISiteIdProvider _siteIdProvider;
        private readonly IGeoCurrency _GeoCurrency;

        public ItineraryController(IQuerySender querySender, IMemoryCache memoryCache
            , IGeoCurrency geoCurrency, ISearchProvider searchProvider, ISiteIdProvider siteIdProvider)
        {
            _querySender = querySender;
            _memoryCache = memoryCache;
            _searchProvider = searchProvider;
            _siteIdProvider = siteIdProvider;
            _GeoCurrency = geoCurrency;
        }

        [HttpGet]
        [Route("api/itinerary/cities")]
        public async Task<SearchResponseViewModel> Get(string searchText)
        {
            var queryResult = await _querySender.Send(new Contracts.Queries.Itinerary.ItineraryQuery
            {
            });
            queryResult.ItinerarySearchData = queryResult.ItinerarySearchData.Where(s => s.CityName != "").GroupBy(x => x.CityName, (key, group) => group.First()).ToList();
            return new SearchResponseViewModel
            {
                ItinerarySerchData = queryResult.ItinerarySearchData
            };
        }

        [HttpGet]
        [Route("api/all/budgetRange")]
        public async Task<MasterBudgetRangeResponseViewModel> GetAllBudgetRange()
        {
            var result = await _querySender.Send(new MasterBudgetRangeQuery());
            result.MasterBudgetRanges = _GeoCurrency.UpdateBudgetRange(result.MasterBudgetRanges, HttpContext);
            return new MasterBudgetRangeResponseViewModel
            {
                MasterBudgetRanges = result.MasterBudgetRanges,
                CurrencyTypes = result.CurrencyTypes
            };
        }

        [HttpGet]
        [Route("api/LocationCategories/{cityIds}")]
        public async Task<CategoryResponseViewModel> GetCategoriesByLocation(string cityIds)
        {
            var SiteId = _siteIdProvider.GetSiteId();
            if (SiteId == Site.DevelopmentSite || Contracts.Enums.Site.FeelDevelopmentSite == SiteId)
            {
                SiteId = Site.feelaplaceSite;
            }
            var eventCategoryResult = await _querySender.Send(new Contracts.Queries.Events.EventCategoryQuery
            {
                Id = 0
            });

            var citiesQueryResult = await _querySender.Send(new Contracts.Queries.Category.CategoriesByLocationQuery
            {
                CityIds = cityIds.Split(',').Select(int.Parse).ToList()
            });

            var eventCategories = eventCategoryResult.EventCategories.Where(c => c.IsFeel == true && c.EventCategoryId == 0).OrderBy(o => o.Order).ToList();
            var Categories = new List<CategoryViewModel>();
            if (eventCategories != null)
            {
                foreach (var item in eventCategories)
                {
                    CategoryViewModel categoryViewModel = new CategoryViewModel();
                    categoryViewModel.DisplayName = item.DisplayName;
                    categoryViewModel.Slug = item.Slug;
                    categoryViewModel.EventCategory = item.Id;
                    categoryViewModel.CategoryId = item.EventCategoryId;
                    categoryViewModel.Order = item.Order;
                    categoryViewModel.IsHomePage = item.IsHomePage;
                    categoryViewModel.IsFeel = item.IsFeel;
                    categoryViewModel.SubCategories = new List<CategoryViewModel>();

                    var SubCategoryList = eventCategoryResult.EventCategories.Where(w => w.EventCategoryId == item.Id).OrderBy(o => o.Order).ToList();
                    SubCategoryList = SubCategoryList.Where(s => citiesQueryResult.EventCategories.Any(x => x.Id == s.Id)).ToList();
                    if (!SubCategoryList.Any())
                    {
                        continue;
                    }
                    CategoryViewModel firstSubCategory = new CategoryViewModel();
                    firstSubCategory.DisplayName = "All";
                    firstSubCategory.Slug = item.Slug;
                    firstSubCategory.EventCategory = 0;
                    firstSubCategory.CategoryId = 0;
                    firstSubCategory.Order = 0;
                    firstSubCategory.IsHomePage = false;
                    firstSubCategory.IsFeel = true;
                    categoryViewModel.SubCategories.Add(firstSubCategory);

                    foreach (var subItem in SubCategoryList)
                    {
                        CategoryViewModel subCategoryViewModel = new CategoryViewModel();
                        subCategoryViewModel.DisplayName = subItem.DisplayName;
                        subCategoryViewModel.Slug = subItem.Slug;
                        subCategoryViewModel.EventCategory = subItem.Id;
                        subCategoryViewModel.CategoryId = subItem.EventCategoryId;
                        subCategoryViewModel.Order = subItem.Order;
                        subCategoryViewModel.IsHomePage = subItem.IsHomePage;
                        subCategoryViewModel.IsFeel = subItem.IsFeel;
                        categoryViewModel.SubCategories.Add(subCategoryViewModel);
                    }
                    if (SubCategoryList.Any())
                    {

                        Categories.Add(categoryViewModel);
                    }
                }
            }
            return new CategoryResponseViewModel
            {
                Categories = Categories,
            };
        }
    }
}
