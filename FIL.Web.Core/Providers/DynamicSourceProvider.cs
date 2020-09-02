using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.Enums;
using FIL.Contracts.Models.DynamicContent;
using FIL.Contracts.Queries;
using FIL.Foundation.Senders;
using FIL.Logging;
using FIL.Web.Core.Utilities;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FIL.Web.Core.Providers
{
    public interface IDynamicSourceProvider
    {
        System.Threading.Tasks.Task<dynamic> GetDynamicContentAsync(dynamic ViewBag);
    }

    public class DynamicSourceProvider : IDynamicSourceProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IQuerySender _querySender;
        private readonly ILogger _logger;
        private readonly ISettings _settings;
        protected readonly ISiteIdProvider _siteIdProvider;
        private readonly ISessionProvider _sessionProvider;

        public DynamicSourceProvider(IHttpContextAccessor httpContextAccessor,
            ISessionProvider sessionProvider,
            IQuerySender querySender, ILogger logger, ISettings settings, ISiteIdProvider siteIdProvider)
        {
            _httpContextAccessor = httpContextAccessor;
            _querySender = querySender;
            _logger = logger;
            _settings = settings;
            _siteIdProvider = siteIdProvider;
            _sessionProvider = sessionProvider;
        }

        public async System.Threading.Tasks.Task<dynamic> GetDynamicContentAsync(dynamic ViewBag)
        {
            string requested_url_path = _httpContextAccessor.HttpContext.Request.Path;
            string requested_url_query = _httpContextAccessor.HttpContext.Request.QueryString.Value;
            string _host = _httpContextAccessor.HttpContext.Request.Host.Host;

            var siteId = _siteIdProvider.GetSiteId();
            if (siteId == Site.ComSite || siteId == Site.DevelopmentSite)
            {
                ViewBag.IsLiveOnline = requested_url_path.Contains("live-online") ? true : false;
                return ViewBag;
            }
            //Get OneSignal AppId 
            string OneSignalAppId = GetOneSignalAppId();
            ViewBag.OneSignalAppId = OneSignalAppId;
            ViewBag.ShouldIndex = true;
            ViewBag.IsPlacePage = false;

            string OgImgUrl = "https://static2.feelitlive.com/images/logos/feel-share-logo.png";
            var shouldRequestContent = ShouldRequestContent(requested_url_path);
            try
            {

                var session = await _sessionProvider.Get();
                if (session.IsAuthenticated & session.User != null)
                {
                    ViewBag.userAltId = session.User.AltId.ToString();
                    ViewBag.userFirstName = session.User.FirstName;
                    ViewBag.userLastName = session.User.LastName;
                    ViewBag.userEmail = session.User.Email;
                }
                if (shouldRequestContent)
                {
                    var queryResult = await _querySender.Send(new DynamicContentQuery { Url = GetRequestedUrl(), QueryString = requested_url_query });

                    bool isFeelLiveHome = (_host.EndsWith(".live") && requested_url_path == "/") ? true : false;
                    //For FeelIt Live
                    if (requested_url_path.Contains("feel-it-live") || isFeelLiveHome)
                    {
                        ViewBag.Title = DynamicContent.LiveOnline.Title;
                        ViewBag.Description = DynamicContent.LiveOnline.Description;
                        OgImgUrl = "https://static2.feelitlive.com/images/logos/fap-live.png";
                    }
                    //For place page
                    if (requested_url_path.Contains("place"))
                    {
                        var placePageContent = GetPlacePageContent(queryResult.DynamicContentResponseModel);
                        ViewBag.Title = placePageContent.Title;
                        ViewBag.Description = placePageContent.Description;
                        ViewBag.ShouldIndex = !string.IsNullOrWhiteSpace(queryResult.DynamicContentResponseModel.PlaceDescription);
                        ViewBag.IsPlacePage = true;
                        ViewBag.ParentCategoryUrl = queryResult.DynamicContentResponseModel.ParentCategoryUrl;
                        ViewBag.SubCategoryUrl = queryResult.DynamicContentResponseModel.SubCategoryUrl;
                        ViewBag.CountryName = queryResult.DynamicContentResponseModel.CountryName;
                        ViewBag.CityName = queryResult.DynamicContentResponseModel.CityName;
                        ViewBag.StateName = queryResult.DynamicContentResponseModel.StateName;
                        ViewBag.VenueName = queryResult.DynamicContentResponseModel.VenueName;
                        ViewBag.Orgin = "https://" + _httpContextAccessor.HttpContext.Request.Host.Value;
                        ViewBag.ParentCategoryName = queryResult.DynamicContentResponseModel.ParentCategoryName;
                        ViewBag.SubCategoryName = queryResult.DynamicContentResponseModel.SubCategoryName;
                        ViewBag.PlaceName = queryResult.DynamicContentResponseModel.PlaceName;
                        OgImgUrl = "https://static1.feelitlive.com/images/places/tiles/" + queryResult.DynamicContentResponseModel.AltId.ToString().ToUpper() + "-ht-c1.jpg";
                        ViewBag.PlaceImage = OgImgUrl;
                        if (queryResult.DynamicContentResponseModel.ParentCategoryName == "Live Stream")
                        {
                            OgImgUrl = "https://static2.feelitlive.com/images/logos/fap-live.png";
                        }
                        ViewBag.EventStartDate = queryResult.DynamicContentResponseModel.ProductMarkupModelList.Count > 0 ? queryResult.DynamicContentResponseModel.ProductMarkupModelList[0].availabilityStarts : DateTime.UtcNow;
                        ViewBag.EventEndDate = queryResult.DynamicContentResponseModel.ProductMarkupModelList.Count > 0 ? queryResult.DynamicContentResponseModel.ProductMarkupModelList[0].availabilityEnds : DateTime.UtcNow;
                        ViewBag.markupString = queryResult.DynamicContentResponseModel.ProductMarkupModelList;
                    }
                    //For Country, City & State Pages
                    if (requested_url_path.Contains("country") && !requested_url_path.Contains("/c/"))
                    {
                        ViewBag.Title = DynamicContent.GenericLocationPage.Title.Replace("locationName", queryResult.DynamicContentResponseModel.GenericLocationName);
                        ViewBag.Description = DynamicContent.GenericLocationPage.Description.Replace("locationName", queryResult.DynamicContentResponseModel.GenericLocationName);
                    }

                    //For Category Landing Pages
                    if (requested_url_path.Contains("/c/"))
                    {
                        //Setting First Country, City & State Landing ones
                        if (requested_url_query.Contains("subcategory") && requested_url_query.Contains("category") && requested_url_query.Contains("country"))
                        {
                            var categoryPageContent = GetCategoryPageContent(queryResult.DynamicContentResponseModel, true, true, true);
                            ViewBag.Title = categoryPageContent.Title;
                            ViewBag.Description = categoryPageContent.Description;
                        }
                        else if (requested_url_query.Contains("subcategory") && requested_url_query.Contains("category") && (requested_url_query.Contains("state") || requested_url_query.Contains("city")))
                        {
                            var categoryPageContent = GetCategoryPageContent(queryResult.DynamicContentResponseModel, true, true, false);
                            ViewBag.Title = categoryPageContent.Title;
                            ViewBag.Description = categoryPageContent.Description;
                        }
                        else if (requested_url_query.Contains("subcategory") && requested_url_query.Contains("category"))
                        {
                            var categoryPageContent = GetCategoryPageContent(queryResult.DynamicContentResponseModel, true, false, false);
                            ViewBag.Title = categoryPageContent.Title;
                            ViewBag.Description = categoryPageContent.Description;
                        }
                        else if (requested_url_query.Contains("category") && requested_url_query.Contains("country"))
                        {
                            var categoryPageContent = GetCategoryPageContent(queryResult.DynamicContentResponseModel, false, true, true);
                            ViewBag.Title = categoryPageContent.Title;
                            ViewBag.Description = categoryPageContent.Description;
                        }
                        else if (requested_url_query.Contains("category") && (requested_url_query.Contains("state") || requested_url_query.Contains("city")))
                        {
                            var categoryPageContent = GetCategoryPageContent(queryResult.DynamicContentResponseModel, false, true, false);
                            ViewBag.Title = categoryPageContent.Title;
                            ViewBag.Description = categoryPageContent.Description;
                        }
                        else if (requested_url_query.Contains("category"))
                        {
                            var categoryPageContent = GetCategoryPageContent(queryResult.DynamicContentResponseModel, false, false, false);
                            ViewBag.Title = categoryPageContent.Title;
                            ViewBag.Description = categoryPageContent.Description;
                        }
                        if (queryResult.DynamicContentResponseModel.ParentCategoryName == "Live Stream")
                        {
                            OgImgUrl = "https://static2.feelitlive.com/images/logos/fap-live.png";
                        }
                    }
                }
                else
                {
                    ViewBag.Title = "FeelitLIVE: Online and in-real-life experiences, places, events";
                    ViewBag.Description = "The world is yours to feel. FeelitLIVE. Discover online and in-real-life experiences, events, and places around the world, or create and host your own.";
                }
            }
            catch (Exception e)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, new Exception(e.Message));
                ViewBag.Orgin = "https://" + _httpContextAccessor.HttpContext.Request.Host.Value;
                ViewBag.Path = requested_url_path + requested_url_query;
                ViewBag.FullUrl = GetRequestedUrl();
                ViewBag.OgImg = OgImgUrl;
                ViewBag.IsLiveOnline = requested_url_path.Contains("live-online") ? true : false;
                ViewBag.isLiveStream = (requested_url_path.Contains("live-event") || requested_url_path.Contains("stream-online")) ? true : false;
                ViewBag.Title = "FeelitLIVE: Online and in-real-life experiences, places, events";
                ViewBag.Description = "The world is yours to feel. FeelitLIVE. Discover online and in-real-life experiences, events, and places around the world, or create and host your own.";
            }
            ViewBag.Orgin = "https://" + _httpContextAccessor.HttpContext.Request.Host.Value;
            ViewBag.Path = requested_url_path + requested_url_query;
            ViewBag.FullUrl = GetRequestedUrl();
            ViewBag.OgImg = OgImgUrl;
            ViewBag.IsLiveOnline = requested_url_path.Contains("live-online") ? true : false;
            ViewBag.isLiveStream = (requested_url_path.Contains("live-event") || requested_url_path.Contains("stream-online")) ? true : false;
            if (string.IsNullOrEmpty(ViewBag.Title) || string.IsNullOrEmpty(ViewBag.Description))
            {
                ViewBag.Title = "FeelitLIVE: Online and in-real-life experiences, places, events";
                ViewBag.Description = "The world is yours to feel. FeelitLIVE. Discover online and in-real-life experiences, events, and places around the world, or create and host your own.";
            }
            return ViewBag;
        }

        private string GetOneSignalAppId()
        {
            string _host = _httpContextAccessor.HttpContext.Request.Host.Host;
            if (!_host.ToLower().Contains("localhost") && !_host.ToLower().Contains("dev"))
            {
                if (_host.ToLower().EndsWith(".com"))
                {
                    return _settings.GetConfigSetting(SettingKeys.Integration.OneSignalAppID.Com).Value.ToString();
                }
                else if (_host.ToLower().EndsWith(".co.uk"))
                {
                    return _settings.GetConfigSetting(SettingKeys.Integration.OneSignalAppID.Uk).Value.ToString();
                }
                else if (_host.ToLower().EndsWith(".co.in"))
                {
                    return _settings.GetConfigSetting(SettingKeys.Integration.OneSignalAppID.India).Value.ToString();
                }
                else if (_host.ToLower().EndsWith(".com.au"))
                {
                    return _settings.GetConfigSetting(SettingKeys.Integration.OneSignalAppID.Aus).Value.ToString();
                }
                else if (_host.ToLower().EndsWith(".fr"))
                {
                    return _settings.GetConfigSetting(SettingKeys.Integration.OneSignalAppID.France).Value.ToString();
                }
                else if (_host.ToLower().EndsWith(".nz"))
                {
                    return _settings.GetConfigSetting(SettingKeys.Integration.OneSignalAppID.NewZealand).Value.ToString();
                }
                else if (_host.ToLower().EndsWith(".de"))
                {
                    return _settings.GetConfigSetting(SettingKeys.Integration.OneSignalAppID.Germany).Value.ToString();
                }
                else if (_host.ToLower().EndsWith(".es"))
                {
                    return _settings.GetConfigSetting(SettingKeys.Integration.OneSignalAppID.Spain).Value.ToString();
                }
                else
                {
                    return String.Empty;
                }
            }
            else
            {
                return String.Empty;
            }
        }

        private bool ShouldRequestContent(string requested_url_query)
        {
            bool flag = false;
            if (!_httpContextAccessor.HttpContext.Request.Host.Value.Contains("localhost") && !requested_url_query.Contains("__webpack_hmr"))
            {
                if (requested_url_query.Contains("place") || requested_url_query.Contains("/c/") || requested_url_query.Contains("country") || requested_url_query.Contains("feel-it-live"))
                {
                    flag = true;
                }
            }
            return flag;
        }

        public PageContentModel GetCategoryPageContent(DynamicContentResponseModel dynamicContentResponseModel, bool isSubCat, bool isLocation, bool isCountry)
        {
            PageContentModel categoryPageContent = new PageContentModel();
            string title = String.Empty;
            string description = string.Empty;
            if (dynamicContentResponseModel.ParentCategoryName == "See & Do")
            {
                if (!isLocation)
                {
                    title = isSubCat ? DynamicContent.CategoryPages.SeeAndDo.SubCategory.Title : DynamicContent.CategoryPages.SeeAndDo.Title;
                    description = isSubCat ? DynamicContent.CategoryPages.SeeAndDo.SubCategory.Description : DynamicContent.CategoryPages.SeeAndDo.Description;
                }
                else
                {
                    if (!isSubCat)
                    {
                        title = isCountry ? DynamicContent.CategoryPages.SeeAndDo.Country.Title : DynamicContent.CategoryPages.SeeAndDo.City.Title;
                        description = isCountry ? DynamicContent.CategoryPages.SeeAndDo.Country.Description : DynamicContent.CategoryPages.SeeAndDo.City.Description;
                    }
                    else
                    {
                        title = isCountry ? DynamicContent.CategoryPages.SeeAndDo.SubCategory.Country.Title : DynamicContent.CategoryPages.SeeAndDo.SubCategory.City.Title;
                        description = isCountry ? DynamicContent.CategoryPages.SeeAndDo.SubCategory.Country.Description : DynamicContent.CategoryPages.SeeAndDo.SubCategory.City.Description;
                    }
                }
            }
            else if (dynamicContentResponseModel.ParentCategoryName == "Eat & Drink")
            {
                if (!isLocation)
                {
                    title = isSubCat ? DynamicContent.CategoryPages.EatAndDrinks.SubCategory.Title : DynamicContent.CategoryPages.EatAndDrinks.Title;
                    description = isSubCat ? DynamicContent.CategoryPages.EatAndDrinks.SubCategory.Description : DynamicContent.CategoryPages.EatAndDrinks.Description;
                }
                else
                {
                    if (!isSubCat)
                    {
                        title = isCountry ? DynamicContent.CategoryPages.EatAndDrinks.Country.Title : DynamicContent.CategoryPages.EatAndDrinks.City.Title;
                        description = isCountry ? DynamicContent.CategoryPages.EatAndDrinks.Country.Description : DynamicContent.CategoryPages.EatAndDrinks.City.Description;
                    }
                    else
                    {
                        title = isCountry ? DynamicContent.CategoryPages.EatAndDrinks.SubCategory.Country.Title : DynamicContent.CategoryPages.EatAndDrinks.SubCategory.City.Title;
                        description = isCountry ? DynamicContent.CategoryPages.EatAndDrinks.SubCategory.Country.Description : DynamicContent.CategoryPages.EatAndDrinks.SubCategory.City.Description;
                    }
                }
            }
            else if (dynamicContentResponseModel.ParentCategoryName == "Shop Local")
            {
                if (!isLocation)
                {
                    title = isSubCat ? DynamicContent.CategoryPages.ShopLocal.SubCategory.Title : DynamicContent.CategoryPages.ShopLocal.Title;
                    description = isSubCat ? DynamicContent.CategoryPages.ShopLocal.SubCategory.Description : DynamicContent.CategoryPages.ShopLocal.Description;
                }
                else
                {
                    if (!isSubCat)
                    {
                        title = isCountry ? DynamicContent.CategoryPages.ShopLocal.Country.Title : DynamicContent.CategoryPages.ShopLocal.City.Title;
                        description = isCountry ? DynamicContent.CategoryPages.ShopLocal.Country.Description : DynamicContent.CategoryPages.ShopLocal.City.Description;
                    }
                    else
                    {
                        title = isCountry ? DynamicContent.CategoryPages.ShopLocal.SubCategory.Country.Title : DynamicContent.CategoryPages.ShopLocal.SubCategory.City.Title;
                        description = isCountry ? DynamicContent.CategoryPages.ShopLocal.SubCategory.Country.Description : DynamicContent.CategoryPages.ShopLocal.SubCategory.City.Description;
                    }
                }
            }
            else if (dynamicContentResponseModel.ParentCategoryName == "Experiences & Activities")
            {
                if (!isLocation)
                {
                    title = isSubCat ? DynamicContent.CategoryPages.ExperiencesAndActivities.SubCategory.Title : DynamicContent.CategoryPages.ExperiencesAndActivities.Title;
                    description = isSubCat ? DynamicContent.CategoryPages.ExperiencesAndActivities.SubCategory.Description : DynamicContent.CategoryPages.ExperiencesAndActivities.Description;
                }
                else
                {
                    if (!isSubCat)
                    {
                        title = isCountry ? DynamicContent.CategoryPages.ExperiencesAndActivities.Country.Title : DynamicContent.CategoryPages.ExperiencesAndActivities.City.Title;
                        description = isCountry ? DynamicContent.CategoryPages.ExperiencesAndActivities.Country.Description : DynamicContent.CategoryPages.ExperiencesAndActivities.City.Description;
                    }
                    else
                    {
                        title = isCountry ? DynamicContent.CategoryPages.ExperiencesAndActivities.SubCategory.Country.Title : DynamicContent.CategoryPages.ExperiencesAndActivities.SubCategory.City.Title;
                        description = isCountry ? DynamicContent.CategoryPages.ExperiencesAndActivities.SubCategory.Country.Description : DynamicContent.CategoryPages.ExperiencesAndActivities.SubCategory.City.Description;
                    }
                }
            }
            else if (dynamicContentResponseModel.ParentCategoryName == "Live Stream")
            {
                title = isSubCat ? DynamicContent.CategoryPages.LiveStream.SubCategory.Title : DynamicContent.CategoryPages.LiveStream.Title;
                description = isSubCat ? DynamicContent.CategoryPages.LiveStream.SubCategory.Description : DynamicContent.CategoryPages.LiveStream.Description;
            }
            string requested_url_query = _httpContextAccessor.HttpContext.Request.QueryString.Value;
            var locationName = requested_url_query.Contains("state") ? dynamicContentResponseModel.StateName : dynamicContentResponseModel.CityName;

            title = title.Replace("SubCategoryName", dynamicContentResponseModel.SubCategoryName).Replace("countryName", dynamicContentResponseModel.CountryName).Replace("cityName", locationName);
            description = description.Replace("SubCategoryName", dynamicContentResponseModel.SubCategoryName).Replace("countryName", dynamicContentResponseModel.CountryName).Replace("cityName", locationName);
            categoryPageContent.Title = title;
            categoryPageContent.Description = description;
            return categoryPageContent;
        }

        public PageContentModel GetPlacePageContent(DynamicContentResponseModel dynamicContentResponseModel)
        {
            PageContentModel placePageContent = new PageContentModel();
            string title = String.Empty;
            string description = string.Empty;
            if (dynamicContentResponseModel.ParentCategoryName == "See & Do")
            {
                title = DynamicContent.PlacePage.SeeAndDoCategory.Title;
                description = DynamicContent.PlacePage.SeeAndDoCategory.Description;
            }
            else if (dynamicContentResponseModel.ParentCategoryName == "Eat & Drink")
            {
                title = DynamicContent.PlacePage.EatAndDrink.Title;
                description = DynamicContent.PlacePage.EatAndDrink.Description;
            }
            else if (dynamicContentResponseModel.ParentCategoryName == "Shop Local")
            {
                title = DynamicContent.PlacePage.ShopLocal.Title;
                description = DynamicContent.PlacePage.ShopLocal.Description;
            }
            else if (dynamicContentResponseModel.ParentCategoryName == "Experiences & Activities")
            {
                title = DynamicContent.PlacePage.ExperiencesAndActivities.Title;
                description = DynamicContent.PlacePage.ExperiencesAndActivities.Description;
            }
            else if (dynamicContentResponseModel.ParentCategoryName == "Live Stream" || dynamicContentResponseModel.MasterEventTypeId == MasterEventType.Online)
            {
                title = DynamicContent.PlacePage.LiveStream.Title;
                description = DynamicContent.PlacePage.LiveStream.Description;
            }

            title = title.Replace("placeName", dynamicContentResponseModel.PlaceName).Replace("cityName", dynamicContentResponseModel.CityName).Replace("countryName", dynamicContentResponseModel.CountryName);
            description = description.Replace("placeName", dynamicContentResponseModel.PlaceName).Replace("cityName", dynamicContentResponseModel.CityName).Replace("countryName", dynamicContentResponseModel.CountryName);

            placePageContent.Title = title;
            placePageContent.Description = description;

            return placePageContent;
        }

        public string GetRequestedUrl()
        {
            string requestUrl = _httpContextAccessor.HttpContext.Request.Host.Value.Contains("localhost") == true ? "http://" + _httpContextAccessor.HttpContext.Request.Host.Value + _httpContextAccessor.HttpContext.Request.Path + _httpContextAccessor.HttpContext.Request.QueryString : "https://" + _httpContextAccessor.HttpContext.Request.Host.Value + _httpContextAccessor.HttpContext.Request.Path + _httpContextAccessor.HttpContext.Request.QueryString;
            return requestUrl;
        }

        public class PageContentModel
        {
            public string Title { get; set; }
            public string Description { get; set; }
        }
    }
}