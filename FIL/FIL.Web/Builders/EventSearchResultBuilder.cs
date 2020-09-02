using System;
using System.Linq;
using FIL.Contracts.Models;
using FIL.Contracts.QueryResults.Events;
using FIL.Web.Feel.Providers;
using FIL.Web.Feel.ViewModels.Search;
using Microsoft.AspNetCore.Http;

namespace FIL.Web.Feel.Builders
{
    public interface IEventSearchResultBuilder
    {
        EventSearchResult Build(CategoryEventContainer container);
        EventSearchResult Build(InfiniteFeelAnalyticsItem item, EventCategoryQueryResult ParentCategory);
    }

    public class EventSearchResultBuilder : IEventSearchResultBuilder
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Logging.ILogger _logger;
        public EventSearchResultBuilder(IHttpContextAccessor httpContextAccessor, Logging.ILogger logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public EventSearchResult Build(CategoryEventContainer container)
        {
            var city = container.City.FirstOrDefault();
            var state = container.State.FirstOrDefault(s => city == null || s.Id == city.StateId);
            EventSearchResult _EventSearchResult = new EventSearchResult
            {
                AltId = container.CategoryEvent.AltId,
                Name = container.CategoryEvent.Name,
                ParentCategory = container.ParentCategory,
                CityName = city?.Name ?? string.Empty,
                CountryName = container.Country.FirstOrDefault(c => state == null || c.Id == state.CountryId)?.Name ?? string.Empty,
                RedirectUrl = "https://" + GetBaseUrl() + "/place/" + container.ParentCategory.Replace("&", "and").Replace(" ", "-").ToLower() + "/" + container.Event.Slug.ToString().ToLower() + "/" + container.EventCategory.Replace("&", "and").Replace(" ", "-").ToLower()
            };
            return _EventSearchResult;
        }

        public EventSearchResult Build(InfiniteFeelAnalyticsItem item, EventCategoryQueryResult ParentCategory)
        {
            try
            {
                UpdateInfiniteFeelAnalyticsItem(item);

                var parentCategory = ParentCategory.EventCategories.Where(w => w.Category == item.Category).FirstOrDefault();

                //Logic to check if parentCategory variable is null or not [Sentry error handling NullReferenceException]
                int parentId = -1;
                if (parentCategory != null)
                {
                    parentId = parentCategory.EventCategoryId;
                }

                var parent = ParentCategory.EventCategories.Where(w => w.Id == parentId).FirstOrDefault();
                if (parent != null)
                {
                    EventSearchResult _EventSearchResult = new EventSearchResult
                    {
                        AltId = new Guid(item.DeDuplicator), // DeDuplicator returning the altIds
                        Name = item.Name,
                        ParentCategory = parent.Category,
                        CityName = item.City,
                        CountryName = item.Country,
                        RedirectUrl = item.Url
                    };
                    return _EventSearchResult;
                }
                else
                {
                    EventSearchResult _EventSearchResult = new EventSearchResult
                    {
                        AltId = new Guid(item.DeDuplicator), // DeDuplicator returning the altIds
                        Name = item.Name,
                        ParentCategory = "SeeAndDo",
                        CityName = item.City,
                        CountryName = item.Country,
                        RedirectUrl = item.Url
                    };
                    return _EventSearchResult;
                }
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                UpdateInfiniteFeelAnalyticsItem(item);
                EventSearchResult _EventSearchResult = new EventSearchResult
                {
                    AltId = new Guid(item.DeDuplicator), // DeDuplicator returning the altIds
                    Name = item.Name,
                    ParentCategory = "SeeAndDo",
                    CityName = item.City,
                    CountryName = item.Country,
                    RedirectUrl = item.Url
                };

                return _EventSearchResult;
            }
        }

        string GetBaseUrl()
        {
            string _host = _httpContextAccessor.HttpContext.Request.Host.Host;

            if (_host.ToLower().Contains("localhost"))
                return _host + ":" + _httpContextAccessor.HttpContext.Request.Host.Port;
            else
                return _host;
        }

        void UpdateInfiniteFeelAnalyticsItem(InfiniteFeelAnalyticsItem item)
        {
            //dont do anything if already contains geo sites for dev or prod
            if (!item.Url.ToLower().Contains("feelitlive.co.uk") && !item.Url.ToLower().Contains("feelitlive.co.in") && !item.Url.ToLower().Contains("feelitlive.com.au") && !item.Url.ToLower().Contains("feelitlive.de") && !item.Url.ToLower().Contains("feelitlive.es") && !item.Url.ToLower().Contains("feelitlive.fr") && !item.Url.ToLower().Contains("feelitlive.co.nz"))
            {
                //else do the geo changes
                if (item.Url.ToLower().Contains("dev.feelitlive.com")) { item.Url = item.Url.ToLower().Replace("dev.feelitlive.com", GetBaseUrl()); }
                else if (item.Url.ToLower().Contains("www.feelitlive.com")) { item.Url = item.Url.ToLower().Replace("www.feelitlive.com", GetBaseUrl()); }
                else if (item.Url.ToLower().Contains("feelitlive.com")) { item.Url = item.Url.ToLower().Replace("feelitlive.com", GetBaseUrl()); }
                //do a localhost test to see all works.
                if (GetBaseUrl().ToLower().Contains("localhost") || GetBaseUrl().ToLower().Contains("dev.feelitlive.com"))
                {
                    //manually removing www. as we dont need it in localhost
                    item.Url = item.Url.ToLower().Replace("www.", string.Empty);
                }
            }
        }

    }
}
