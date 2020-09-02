using System;
using System.Linq;
using System.Threading.Tasks;
using FIL.Web.Feel.ViewModels.EventLearnPage;
using Microsoft.AspNetCore.Mvc;
using FIL.Foundation.Senders;
using FIL.Contracts.Queries.FeelEventLearnPage;
using FIL.Contracts.Enums;
using FIL.Web.Feel.ViewModels.Category;
using FIL.Contracts.Queries.Category;
using System.Collections.Generic;
using FIL.Web.Core.Providers;
using FIL.Web.Core;
using FIL.Web.Feel.ViewModels.CategoryHomePage;
using FIL.Configuration;
using FIL.Web.Feel.Modules.SiteExtensions;
using FIL.Logging;
using FIL.Configuration.Utilities;

namespace FIL.Web.Feel.Controllers
{
    public class EventLearnPageController : Controller
    {
        private readonly IQuerySender _querySender;
        private readonly ISiteIdProvider _siteIdProvider;
        private readonly IAmazonS3FileProvider _amazonS3FileProvider;
        private readonly ISettings _settings;
        private readonly IGeoCurrency _geoCurrency;
        private readonly ILogger _logger;

        public EventLearnPageController(IQuerySender querySender,
            IAmazonS3FileProvider amazonS3FileProvider, ISettings settings,
            ISiteIdProvider siteIdProvider, IGeoCurrency geoCurrency, ILogger logger)
        {
            _logger = logger;
            _querySender = querySender;
            _siteIdProvider = siteIdProvider;
            _amazonS3FileProvider = amazonS3FileProvider;
            _settings = settings;
            _geoCurrency = geoCurrency;
            _logger = logger;
        }

        [HttpPost]
        [Route("api/event/slug")]
        public async Task<EventLearnPageResponseViewModel> Get([FromBody]CategoryEventViewModel model)
        {
            var siteId = _siteIdProvider.GetSiteId();
            siteId = siteId == Site.DevelopmentSite ? Site.feelaplaceSite : siteId;
            if (model == null)
            {
                return new EventLearnPageResponseViewModel { };
            }
            var queryResult = await _querySender.Send(new FeelEventLearnPageQuery
            {
                Slug = model.slug,
                EventAltId = model.EventAltId
            });

            var eventCategoryResult = await _querySender.Send(new Contracts.Queries.Events.EventCategoryQuery
            {
                Id = 0
            });

            if ((eventCategoryResult == null && eventCategoryResult.EventCategories == null) || queryResult.Event == null)
            {
                return new EventLearnPageResponseViewModel { };
            }

            var eventCategory = eventCategoryResult.EventCategories.Where(w => w.Id == queryResult.EventCategory.Id).FirstOrDefault();

            var eventParent = eventCategoryResult.EventCategories.Where(w => w.Id == eventCategory.EventCategoryId).FirstOrDefault();

            var categoryName = eventParent != null ? eventParent.DisplayName : string.Empty;

            Contracts.QueryResults.Category.FeelCategoryEventQueryResult similarListings = null;

            try
            {
                similarListings = await _querySender.Send(new FeelCategoryEventQuery
                {
                    EventCategoryId = eventCategory.Id,
                    SiteId = (Site)siteId,
                    IsSimilarListing = true,
                    IsCountryLandingPage = false,
                    IsCityLandingPage = false
                });

                foreach (var item in similarListings.CategoryEvents)
                {
                    var categoryData = eventCategoryResult.EventCategories.Where(w => w.Id.ToString() == item.EventCategory).FirstOrDefault();
                    if (categoryData != null)
                    {
                        if (item.EventCategory != null)
                        {
                            item.EventCategory = categoryData != null ? categoryData.DisplayName.Replace("&", "and").Replace(" ", "-").ToLower() : string.Empty;
                        }
                        var parentCategoryData = eventCategoryResult.EventCategories.Where(w => w.Id == categoryData.EventCategoryId).FirstOrDefault();
                        if (item.ParentCategory != null)
                        {
                            item.ParentCategory = parentCategoryData != null ? parentCategoryData.DisplayName.Replace("&", "and").Replace(" ", "-").ToLower() : string.Empty;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                similarListings = null;
            }

            //override price to local currency.
            _geoCurrency.UpdateCategoriesCurrency_v2_2(HttpContext, queryResult);

            if (similarListings != null)
                _geoCurrency.UpdateCategoriesCurrency_v2(similarListings.CategoryEvents, HttpContext);
            else
                _geoCurrency.UpdateCategoriesCurrency_v2(new List<Contracts.Models.CategoryEventContainer>(), HttpContext);
            //override ends

            List<FIL.Contracts.Models.UserImageMap> userImageMaps = new List<Contracts.Models.UserImageMap>();
            foreach (var user in queryResult.Rating.ToList())
            {
                FIL.Contracts.Models.UserImageMap imageMap = new Contracts.Models.UserImageMap();
                imageMap.Id = user.UserId;
                var userInformation = queryResult.User.ToList().Where(userInfo => userInfo.Id == user.UserId).FirstOrDefault();
                imageMap.ImagePath = GetProfilePicture(userInformation.AltId);
                userImageMaps.Add(imageMap);
            }

            return new EventLearnPageResponseViewModel
            {
                Categories = (similarListings != null ? similarListings.CategoryEvents : null),
                CurrencyType = queryResult.CurrencyType,
                Event = queryResult.Event,
                EventCategory = eventCategory.DisplayName.ToString(),
                EventType = queryResult.EventType.ToString(),
                EventDetail = queryResult.EventDetail,
                Country = queryResult.Country,
                State = queryResult.State,
                City = queryResult.City,
                Venue = queryResult.Venue,
                EventTicketAttribute = queryResult.EventTicketAttribute,
                EventTicketDetail = queryResult.EventTicketDetail,
                Rating = queryResult.Rating,
                TicketCategory = queryResult.TicketCategory,
                User = queryResult.User,
                EventAmenitiesList = queryResult.EventAmenitiesList,
                ClientPointOfContact = queryResult.ClientPointOfContact,
                EventGalleryImage = queryResult.EventGalleryImage,
                EventCategoryName = categoryName,
                EventLearnMoreAttributes = queryResult.EventLearnMoreAttributes,
                UserImageMap = userImageMaps,
                RegularTimeModel = queryResult.RegularTimeModel,
                SeasonTimeModel = queryResult.SeasonTimeModel,
                SpecialDayModel = queryResult.SpecialDayModel,
                CitySightSeeingRoutes = queryResult.CitySightSeeingRoutes,
                CitySightSeeingRouteDetails = queryResult.CitySightSeeingRouteDetails,
                TiqetsCheckoutDetails = queryResult.TiqetsCheckoutDetails,
                Category = queryResult.Category,
                SubCategory = queryResult.SubCategory,
                EventHostMappings = queryResult.EventHostMappings,
                OnlineStreamStartTime = queryResult.OnlineStreamStartTime,
                OnlineEventTimeZone=queryResult.OnlineEventTimeZone,
                TicketAlertEventMapping=queryResult.TicketAlertEventMapping
            };
        }
        public string GetProfilePicture(Guid altId)
        {
            var userAltId = altId.ToString();
            var baseURL = _siteIdProvider.GetSiteId() == Site.FeelDevelopmentSite ? _settings.GetConfigSetting(SettingKeys.Aws.S3.PathName) + "/" + _settings.GetConfigSetting(SettingKeys.Aws.S3.Feel.BucketName).Value : _settings.GetConfigSetting(SettingKeys.Aws.S3.Feel.StaticURL).Value.Split(",").ElementAt(0);
            try
            {
                return baseURL + "/images/user-profile/fee-review-user-icon.png";
            }
            catch
            {
                return baseURL + "/images/user-profile/fee-review-user-icon.png";
            }
        }

        [HttpGet]
        [Route("api/event/category/{categorySlug}")]
        public async Task<CategoryPageResponseViewModel> GetCategory(string categorySlug)
        {
            var eventCategoryResult = await _querySender.Send(new Contracts.Queries.Events.EventCategoryQuery
            {
                Slug = categorySlug
            });

            var category = new CategoryViewModel
            {
                DisplayName = eventCategoryResult.EventCategories[0].DisplayName,
                Slug = eventCategoryResult.EventCategories[0].Slug,
                EventCategory = eventCategoryResult.EventCategories[0].Id,
                Order = eventCategoryResult.EventCategories[0].Order,
                IsHomePage = eventCategoryResult.EventCategories[0].IsHomePage,
                IsFeel = eventCategoryResult.EventCategories[0].IsFeel,
                CategoryId = eventCategoryResult.EventCategories[0].EventCategoryId
            };

            return new CategoryPageResponseViewModel
            {
                Category = category
            };
        }

        [HttpGet]
        [Route("api/weather/getconfig")]
        public async Task<string> getWeatherConfig()
        {
            var apikey = _settings.GetConfigSetting("AccuWeatherAPIKey");
            return apikey.Value;
        }
    }
}
