using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Autofac.Core;
using Autofac.Features.AttributeFilters;
using Flurl;
using FIL.Contracts.Attributes;
using FIL.Contracts.Enums;
using FIL.Foundation.Senders;
using FIL.Http;
using FIL.Logging.Enums;
using FIL.Utilities.Extensions;
using FIL.Web.Core.Providers;
using FIL.Web.Feel.Builders;
using FIL.Web.Feel.ViewModels.Search;
using Newtonsoft.Json;

namespace FIL.Web.Feel.Providers
{
    public class InfiniteFeelAnalyticsItem
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }
        [JsonProperty("de-duplicator")]
        public string DeDuplicator { get; set; }
        public string Description { get; set; }
        public string Index { get; set; }
        public string Url { get; set; }
        [JsonProperty("availability_status")]
        public bool AvailabilityStatus { get; set; }
        public string Series { get; set; }
        public double Price { get; set; }
        public string Name { get; set; }
        public int Sku { get; set; }
        public string Category { get; set; }
        [JsonProperty("site_product_id")]
        public Guid SiteProductId { get; set; }
    }

    public class InfiniteAnalyticsSearchResult
    {
        public IList<InfiniteFeelAnalyticsItem> Items { get; set; }
    }

    public class InfitieAnalyticsResponseWrapper
    {
        public IList<InfiniteAnalyticsSearchResult> Data { get; set; }
        [JsonProperty("request_id")]
        public Guid RequestId { get; set; }
        public int Status { get; set; }
    }

    public interface ISearchProvider
    {
        Task<IList<EventSearchResult>> Search(string search, Site siteId, SiteLevel siteLevel, bool isAdvanceSearch = false);
        //Task<IList<EventSearchResult>> Search(EventCategory category, Site siteId, SiteLevel siteLevel);
    }

    public class SearchProvider : ISearchProvider
    {
        private readonly IRestHelper _restHelper;
        private readonly ISessionProvider _sessionProvider;
        private readonly Logging.ILogger _logger;
        private readonly IEventSearchResultBuilder _searchResultBuilder;
        private readonly IQuerySender _querySender;


        public SearchProvider(ISessionProvider sessionProvider,
            [KeyFilter("SearchRestHelper")] IRestHelper restHelper,
            Logging.ILogger logger,
            IEventSearchResultBuilder searchResultBuilder,
            IQuerySender querySender)
        {
            _restHelper = restHelper;
            _sessionProvider = sessionProvider;
            _logger = logger;
            _searchResultBuilder = searchResultBuilder;
            _querySender = querySender;

        }

        public async Task<IList<EventSearchResult>> Search(string search, Site siteId, SiteLevel siteLevel, bool isAdvanceSearch = false)
        {
            var result = await SearchBase(search, siteId, siteLevel);
            var eventCategoryResult = await _querySender.Send(new Contracts.Queries.Events.EventCategoryQuery
            {
                Id = 0
            });
            List<EventSearchResult> data = new List<EventSearchResult>();
            if (!isAdvanceSearch)
            {
                data = result.Take(15).Select(kvp => _searchResultBuilder.Build(kvp.Value.First(), eventCategoryResult)).ToList();
            }
            else
            {
                data = result.Select(kvp => _searchResultBuilder.Build(kvp.Value.First(), eventCategoryResult)).ToList();
            }
            return data;
        }

        private async Task<Dictionary<string, List<InfiniteFeelAnalyticsItem>>> SearchBase(string search, Site siteId, SiteLevel siteLevel)
        {
            var url = new Url("SocialGenomix/recommendations/any");
            var session = await _sessionProvider.Get();
            url.QueryParams.Add("session_id", session.AltId);
            url.QueryParams.Add("ecompany", "feelitlive.com");
            url.QueryParams.Add("site_page_type", "search");
            url.QueryParams.Add("client_type", "demo"); // TODO: XXX: demo?
            url.QueryParams.Add("user_type", session.IsAuthenticated ? "user" : "session");
            url.QueryParams.Add("count", 500); // TODO: XXX: count effects
            url.QueryParams.Add("search_string", BuildSearchString(search, siteId, siteLevel));
            if (session.IsAuthenticated && session.User != null)
            {
                url.QueryParams.Add("user_id", session.User.AltId);
            }

            var result = await _restHelper.GetResult<InfitieAnalyticsResponseWrapper>(url.ToString());
            return result.Data.FirstOrDefault()?.Items.GroupBy(r => r.DeDuplicator).ToDictionary(k => k.Key, k => k.ToList())
                ?? new Dictionary<string, List<InfiniteFeelAnalyticsItem>>();
        }

        //public async Task<IList<EventSearchResult>> Search(EventCategory category, Site siteId, SiteLevel siteLevel)
        //{
        //    // TODO: paging
        //    // TODO: XXX: min / max pricing - maybe a category event container?
        //    var search = await SearchBase($"product_category={category.ToString()}", siteId, siteLevel);
        //    return search.Select(kvp => _searchResultBuilder.Build(kvp.Value.First())).ToList();
        //}

        private string BuildSearchString(string search, Site siteId, SiteLevel siteLevel)
        {
            if (siteLevel == SiteLevel.Global || siteLevel == SiteLevel.None)
            {
                return search;
            }

            var (field, value) = GetSiteFilter(siteId, siteLevel);
            var filterString = $"{field}={value.Replace(" ", "_")}";
            return value.Equals(search, StringComparison.InvariantCultureIgnoreCase)
                ? filterString : $"{search} {filterString}";
        }

        private (string Field, string Value) GetSiteFilter(Site siteId, SiteLevel siteLevel)
        {
            var levelValue = siteId.GetAttribute<SearchTermAttribute>()?.Term;
            if (levelValue == null)
            {
                // try to extract from name
                var site = siteId.ToString().Replace("feel", "").Replace("Site", "");
                levelValue = Regex.Replace(site, "([A-Z])", " $1", RegexOptions.Compiled).Trim();
                _logger.Log(LogCategory.Warn, $"Feel site {siteId} does not have SearchTerm set on enum.");
            }
            var field = siteLevel.GetAttribute<SearchTermAttribute>()?.Term ?? siteLevel.ToString().ToLowerInvariant();
            return (field, levelValue);
        }
    }
}

