using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FIL.Contracts.Commands.Blogs;
using FIL.Contracts.Enums;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.Category;
using FIL.Contracts.Queries.FeelCustomDefaultContent;
using FIL.Contracts.Queries.FeelSearch;
using FIL.Contracts.QueryResults.FeelSearch;
using FIL.Foundation.Senders;
using FIL.Web.Core;
using FIL.Web.Core.Providers;
using FIL.Web.Feel.Modules.SiteExtensions;
using FIL.Web.Feel.Providers;
using FIL.Web.Feel.ViewModels.Category;
using FIL.Web.Feel.ViewModels.CategoryHomePage;
using FIL.Web.Feel.ViewModels.SiteContent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace FIL.Web.Feel.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IQuerySender _querySender;
        private readonly ISearchProvider _searchProvider;
        private readonly IMemoryCache _memoryCache;
        private readonly ISiteIdProvider _siteIdProvider;
        private readonly IGeoCurrency _GeoCurrency;
        private readonly FIL.Logging.ILogger _logger;
        private readonly ICommandSender _commandSender;
        private readonly IAmazonS3FileProvider _amazonS3FileProvider;

        public CategoryController(IQuerySender querySender,
            IMemoryCache memoryCache,
            ISearchProvider searchProvider,
            ISiteIdProvider siteIdProvider,
            FIL.Logging.ILogger logger,
            IGeoCurrency GeoCurrency,
            ICommandSender commandSender,
            IAmazonS3FileProvider amazonS3FileProvider)
        {
            _querySender = querySender;
            _memoryCache = memoryCache;
            _searchProvider = searchProvider;
            _siteIdProvider = siteIdProvider;
            _GeoCurrency = GeoCurrency;
            _logger = logger;
            _commandSender = commandSender;
            _amazonS3FileProvider = amazonS3FileProvider;
        }

        [HttpGet]
        [Route("api/category")]
        public async Task<CategoryResponseViewModel> Get()
        {
            var SiteId = _siteIdProvider.GetSiteId();
            if (SiteId == Contracts.Enums.Site.DevelopmentSite || Contracts.Enums.Site.FeelDevelopmentSite == SiteId)
            {
                SiteId = Contracts.Enums.Site.feelaplaceSite;
            }

            if (!_memoryCache.TryGetValue($"{SiteId}_all_categories", out List<CategoryViewModel> Categories))
            {
                var eventCategoryResult = await _querySender.Send(new Contracts.Queries.Events.EventCategoryQuery
                {
                    Id = 0
                });

                var onlineEventCategories = eventCategoryResult.EventCategories.Where(c => c.IsFeel && c.MasterEventTypeId == MasterEventType.Online && c.EventCategoryId == 0).OrderBy(o => o.Order).ToList();
                var offlineEventCategories = eventCategoryResult.EventCategories.Where(c => c.IsFeel && c.MasterEventTypeId != MasterEventType.Online && c.EventCategoryId == 0).OrderBy(o => o.Order).ToList();
                var eventCategories = onlineEventCategories.Concat(offlineEventCategories);
                Categories = new List<CategoryViewModel>();
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
                        categoryViewModel.MasterEventTypeId = item.MasterEventTypeId;
                        categoryViewModel.SubCategories = new List<CategoryViewModel>();

                        var SubCategoryList = eventCategoryResult.EventCategories.Where(w => w.EventCategoryId == item.Id).OrderBy(o => o.Order).ToList();

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
                        Categories.Add(categoryViewModel);
                    }
                    _memoryCache.Set($"{SiteId}_all_categories", Categories, DateTime.Now.AddMinutes(20));
                }
            }

            return new CategoryResponseViewModel
            {
                Categories = Categories,
            };
        }

        DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            return new DateTime((dt.Ticks + d.Ticks - 1) / d.Ticks * d.Ticks, dt.Kind);
        }

        [HttpGet]
        [Route("api/category/{slug}")]
        public async Task<CategoryEventResponseDataViewModel> Get(string slug)
        {
            var SiteId = _siteIdProvider.GetSiteId();
            if (SiteId == Contracts.Enums.Site.DevelopmentSite || Contracts.Enums.Site.FeelDevelopmentSite == SiteId)
            {
                SiteId = Contracts.Enums.Site.feelaplaceSite;
            }

            EventCategory eventCategory = new EventCategory();
            List<EventCategory> eventCategoryList = new List<EventCategory>();
            var eventCategoryResult = await _querySender.Send(new Contracts.Queries.Events.EventCategoryQuery
            {
                Id = 0
            });

            bool isAll = false, isAllOnline = false;
            if (slug.IndexOf("all-top-") > -1)
            {
                eventCategory = eventCategoryResult.EventCategories.Where(w => w.Id == Convert.ToInt32(slug.Replace("all-top-", ""))).FirstOrDefault();
                if (eventCategory.MasterEventTypeId == MasterEventType.Online)
                {
                    isAllOnline = true;
                    eventCategoryList = eventCategoryResult.EventCategories.Where(s => s.MasterEventTypeId == MasterEventType.Online && s.EventCategoryId == 0).ToList();
                }
                isAll = true;
            }
            else
            {
                if (eventCategoryResult != null)
                {
                    eventCategory = eventCategoryResult.EventCategories.Where(w => w.Slug == slug).FirstOrDefault();
                }
            }

            if (eventCategory == null)
            {
                eventCategory = eventCategoryResult.EventCategories.Where(w => w.IsFeel == true).FirstOrDefault();
            }

            var category = new CategoryViewModel
            {
                DisplayName = eventCategory.DisplayName,
                Slug = eventCategory.Slug,
                EventCategory = eventCategory.Id,
                CategoryId = eventCategory.EventCategoryId,
                Order = eventCategory.Order,
                IsHomePage = eventCategory.IsHomePage,
                IsFeel = eventCategory.IsFeel,
                MasterEventTypeId = eventCategory.MasterEventTypeId
            };

            //update cache to save data for each geo
            string TargetCurrencyCode = _GeoCurrency.GetSessionCurrency(HttpContext);
            if (!_memoryCache.TryGetValue($"{TargetCurrencyCode}_category_event_{category.EventCategory}_{SiteId}", out List<FIL.Web.Feel.ViewModels.CategoryHomePage.CategoryEventContainer> categories))
            {
                try
                {
                    var queryResult = await _querySender.Send(new FeelCategoryEventQuery
                    {
                        EventCategoryId = isAll ? Convert.ToInt32(slug.Replace("all-top-", "")) : category.EventCategory,
                        IsAll = isAll,
                        IsAllOnline = isAllOnline,
                        EventCategories = eventCategoryList,
                        SiteId = _siteIdProvider.GetSiteId(),
                        PageNumber = 0,
                        IsCountryLandingPage = false,
                        IsCityLandingPage = false
                    });

                    categories = AutoMapper.Mapper.Map<List<FIL.Web.Feel.ViewModels.CategoryHomePage.CategoryEventContainer>>(queryResult.CategoryEvents);

                    //overwrite categories events to currency selected on UI
                    categories = _GeoCurrency.UpdateCategoriesCurrency(categories, HttpContext);

                    foreach (var item in categories)
                    {
                        //Calculate duration if Events are live Online only.
                        string duration = string.Empty;
                        if (category.MasterEventTypeId == MasterEventType.Online)
                        {
                            var currentEventDetailModel = queryResult.CategoryEvents.Where(s => s.Event.Id == item.Event.Id).FirstOrDefault().EventDetail.FirstOrDefault();

                            try
                            {
                                if (item.LiveEventDetail != null && item.EventFrequencyType == EventFrequencyType.OnDemand)
                                {
                                    var timediff = ((DateTime)RoundUp(item.LiveEventDetail.EventStartDateTime.Value, TimeSpan.FromMinutes(10))).Subtract(RoundUp(currentEventDetailModel.StartDateTime, TimeSpan.FromMinutes(10)));
                                    duration = string.Format("{0}:{1}", timediff.Hours, timediff.Minutes);
                                    item.Duration = duration;
                                }
                                else
                                {
                                    var timediff = RoundUp(currentEventDetailModel.EndDateTime, TimeSpan.FromMinutes(10)).Subtract(RoundUp(currentEventDetailModel.StartDateTime, TimeSpan.FromMinutes(10)));
                                    duration = string.Format("{0}:{1}", timediff.Hours, timediff.Minutes);
                                    item.Duration = duration;
                                }
                            }
                            catch (Exception e)
                            {
                                var timediff = currentEventDetailModel.EndDateTime.Subtract(currentEventDetailModel.StartDateTime);
                                duration = string.Format("{0}:{1}", timediff.Hours, timediff.Minutes);
                                item.Duration = duration;
                            }
                            item.EventFrequencyType = item.EventFrequencyType;
                        }
                    }
                    //update geo currency
                    categories = _GeoCurrency.UpdateCategoriesCurrency(categories, HttpContext); //need to filter the categroies list so new filtered cateroies is returned above.
                    //save to cache in the end
                    _memoryCache.Set($"{TargetCurrencyCode}_category_event_{category.EventCategory}_{SiteId}", categories, DateTime.Now.AddMinutes(10));
                }
                catch (Exception e)
                {
                    _logger.Log(Logging.Enums.LogCategory.Error, e);
                    return null;
                }
            }
            return new CategoryEventResponseDataViewModel
            {
                Category = category,
                CategoryEvents = categories,
            };
        }

        [HttpGet]
        [Route("api/placeByIndex/{index}")]
        public async Task<CategoryEventResponseDataViewModel> Get(int index, string slug)
        {
            var SiteId = _siteIdProvider.GetSiteId();

            if (SiteId == Contracts.Enums.Site.DevelopmentSite || Contracts.Enums.Site.FeelDevelopmentSite == SiteId)
            {
                SiteId = Contracts.Enums.Site.feelaplaceSite;
            }
            var eventCategoryResult = await _querySender.Send(new Contracts.Queries.Events.EventCategoryQuery
            {
                Id = 0
            });
            string[] slugName = slug.Split("?");
            string currentSlug = "all-top-29";
            bool isFilter = false;
            string search = "";
            if (slugName.Length > 0)
            {
                slug = slugName[0];
                currentSlug = slugName[0];
                if (slugName[1] != "")
                {
                    isFilter = true;
                    search = slugName[1];
                }
            }
            EventCategory eventCategory = new EventCategory();
            bool isAll = false;
            if (slug.IndexOf("all-top-") > -1)
            {
                eventCategory = eventCategoryResult.EventCategories.Where(w => w.Id == Convert.ToInt32(currentSlug.Replace("all-top-", ""))).FirstOrDefault();
                isAll = true;
            }
            else
            {
                eventCategory = eventCategoryResult.EventCategories.Where(w => w.Slug == currentSlug).FirstOrDefault();
            }
            var category = new CategoryViewModel
            {
                DisplayName = eventCategory.DisplayName,
                Slug = eventCategory.Slug,
                EventCategory = eventCategory.Id,
                CategoryId = eventCategory.EventCategoryId,
                Order = eventCategory.Order,
                IsHomePage = eventCategory.IsHomePage,
                IsFeel = eventCategory.IsFeel
            };
            //update cache to save data for each geo
            string TargetCurrencyCode = _GeoCurrency.GetSessionCurrency(HttpContext);
            if (!_memoryCache.TryGetValue($"{TargetCurrencyCode}_category_placeByIndex_{category.EventCategory}_{SiteId}", out List<FIL.Web.Feel.ViewModels.CategoryHomePage.CategoryEventContainer> categories))
            {
                try
                {
                    var queryResult = await _querySender.Send(new FeelCategoryEventQuery
                    {
                        EventCategoryId = isAll ? Convert.ToInt32(currentSlug.Replace("all-top-", "")) : category.EventCategory,
                        IsAll = isAll,
                        PageNumber = index,
                        Search = search,
                        IsSearch = isFilter,
                        SiteId = _siteIdProvider.GetSiteId(),
                        IsCountryLandingPage = false,
                        IsCityLandingPage = false
                    });
                    categories = AutoMapper.Mapper.Map<List<FIL.Web.Feel.ViewModels.CategoryHomePage.CategoryEventContainer>>(queryResult.CategoryEvents);
                    foreach (var item in categories)
                    {
                        if (item.Event != null)
                        {
                            if (isAll)
                            {
                                var categoryName = category != null ? category.DisplayName : string.Empty;
                                categoryName = categoryName.Replace("&", "and").Replace(" ", "-").Replace("/", "-");
                                item.EventCategory = item.EventCategories[0].Count() > 0 ? item.EventCategories[0].Replace("&", "and").Replace(" ", "-").Replace("/", "-").ToLower() : "other";
                                item.ParentCategory = categoryName.ToLower();
                            }
                            else
                            {
                                var categoryData = eventCategoryResult.EventCategories.Where(w => w.Id == category.CategoryId).ToList();
                                var categoryName = categoryData != null ? categoryData[0].DisplayName : string.Empty;
                                categoryName = categoryName.Replace("&", "and").Replace(" ", "-").Replace("/", "-");
                                item.EventCategory = category.DisplayName.Replace("&", "and").Replace(" ", "-").Replace("/", "-").ToLower();
                                item.ParentCategory = categoryName.ToLower();
                            }
                        }
                    }
                    //update geo currency
                    categories = _GeoCurrency.UpdateCategoriesCurrency(categories, HttpContext);
                    _memoryCache.Set($"{TargetCurrencyCode}_category_placeByIndex_{category.EventCategory}_{SiteId}", categories, DateTime.Now.AddMinutes(10));
                }
                catch (Exception e)
                {
                    _logger.Log(Logging.Enums.LogCategory.Error, e);
                    return null;
                }
            }
            return new CategoryEventResponseDataViewModel
            {
                Category = category,
                CategoryEvents = categories,
            };
        }

        [HttpGet]
        [Route("api/searchcategory/{searchText}")]
        public async Task<CategoryEventResponseDataViewModel> GetSearchData(string searchText, SiteLevel? siteLevel = null)
        {
            // TODO: XXX: NSP: aggregate both searches into parallel queries
            var siteId = _siteIdProvider.GetSiteId();
            if (siteId == Contracts.Enums.Site.DevelopmentSite || Contracts.Enums.Site.FeelDevelopmentSite == siteId)
            {
                siteId = Contracts.Enums.Site.feelaplaceSite;
            }
            var eventCategoryResult = await _querySender.Send(new Contracts.Queries.Events.EventCategoryQuery
            {
                Id = 0
            });

            var eventCategory = eventCategoryResult.EventCategories.Where(w => w.Slug == "monuments").FirstOrDefault();

            var category = new CategoryViewModel
            {
                DisplayName = eventCategory.DisplayName,
                Slug = eventCategory.Slug,
                EventCategory = eventCategory.Id,
                CategoryId = eventCategory.EventCategoryId,
                Order = eventCategory.Order,
                IsHomePage = eventCategory.IsHomePage,
                IsFeel = eventCategory.IsFeel
            };

            siteLevel = siteLevel ?? SiteLevel.Global;
            List<Guid> PlaceAltIds = new List<Guid>();
            var searchResults = await _searchProvider.Search(searchText, siteId, siteLevel.Value, true);
            ////// TODO: XXX: NSP: Search / Site level caching in Redis (not local) - can also derive all locations from IA

            if (searchResults.Count() > 0)
            {
                PlaceAltIds = searchResults.Select(s => s.AltId).ToList();
            }

            FeelSearchQueryResult queryResult;
            List<Contracts.Models.CategoryEventContainer> sortedFeelSearchResult = new List<Contracts.Models.CategoryEventContainer>();
            try
            {
                queryResult = await _querySender.Send(new FeelSearchQuery
                {
                    Name = searchText,
                    IsAdvanceSearch = true,
                    PlaceAltIds = PlaceAltIds,
                    SiteId = siteId,
                    SiteLevel = SiteLevel.Global
                });
                List<FIL.Contracts.Models.CategoryEventContainer> categoryEventContainers = new List<Contracts.Models.CategoryEventContainer>();
                foreach (var item2 in queryResult.FeelAdvanceSearchQueryResult.CategoryEvents)
                {
                    if (item2.Event != null)
                    {
                        categoryEventContainers.Add(item2);
                    }
                }
                sortedFeelSearchResult = categoryEventContainers.OrderBy(d => PlaceAltIds.IndexOf(d.Event.AltId)).ToList();
            }
            catch (NullReferenceException ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
            }

            List<FIL.Web.Feel.ViewModels.CategoryHomePage.CategoryEventContainer> categories = AutoMapper.Mapper.Map<List<FIL.Web.Feel.ViewModels.CategoryHomePage.CategoryEventContainer>>(sortedFeelSearchResult);

            foreach (var item in categories)
            {
                var categoryData = eventCategoryResult.EventCategories.Where(w => w.Id == category.CategoryId).ToList();

                var categoryName = categoryData != null ? categoryData[0].DisplayName : string.Empty;
                categoryName = categoryName.Replace("&", "and");
                categoryName = categoryName.Replace(" ", "-");
                categoryName = categoryName.Replace("/", "-");
                item.EventCategory = category.DisplayName.Replace("&", "and").Replace(" ", "-").Replace("/", "-").ToLower();
                item.ParentCategory = categoryName.ToLower();
            }

            //update geo currency
            categories = _GeoCurrency.UpdateCategoriesCurrency(categories, HttpContext);

            return new CategoryEventResponseDataViewModel
            {
                Category = category,
                CategoryEvents = categories,
            };
        }

        [HttpGet]
        [Route("api/content")]
        public async Task<SiteContentViewModel> GetContent()
        {
            var siteId = _siteIdProvider.GetSiteId();
            if (Contracts.Enums.Site.DevelopmentSite == siteId || Contracts.Enums.Site.FeelDevelopmentSite == siteId)
            {
                siteId = Contracts.Enums.Site.feelaplaceSite;
            }
            if (!_memoryCache.TryGetValue($"content_{siteId}", out FIL.Contracts.QueryResults.FeelCustomDefaultContent.FeelCustomDefaultContentQueryResult queryResult))
            {
                queryResult = await _querySender.Send(new FeelCustomDefaultContentQuery
                {
                    SiteId = siteId,
                });
                _memoryCache.Set($"content_{siteId}", queryResult, DateTime.Now.AddMinutes(30));
            }
            return new SiteContentViewModel
            {
                Content = queryResult.FeelSiteContent,
                SiteBanners = queryResult.SiteBanners,
                DefaultSearchCities = queryResult.Cities,
                DefaultSearchStates = queryResult.States,
                DefaultSearchCountries = queryResult.Countries,
            };
        }
        [HttpGet]
        [Route("api/get/blogs")]
        public async Task<BlogResponseViewModel> GetBlogs()
        {
            if (!_memoryCache.TryGetValue($"FILHPblogs", out GetBlogCommandResult response))
            {
                response = await _commandSender.Send<GetBlogCommand, GetBlogCommandResult>(new GetBlogCommand { ModifiedBy = new Guid("D21A85EE-351C-4349-9953-FE1492740976") }, new TimeSpan(2, 0, 0));

                //Check if Image upload needed
                if (response.BlogResponseModelList.Where(s => Convert.ToBoolean(s.IsImageUpload)).Count() > 0)
                {
                    await UploadImages(response.BlogResponseModelList.Where(s => Convert.ToBoolean(s.IsImageUpload)).ToList());
                }
                _memoryCache.Set($"FILHPblogs", response, DateTime.Now.AddMinutes(30));

            }
            return new BlogResponseViewModel
            {
                BlogResponseModelList = response.BlogResponseModelList
            };
        }

        public async Task UploadImages(List<BlogResponseModel> blogResponseModelList)
        {
            System.Net.WebClient wc = new System.Net.WebClient();
            foreach (var item in blogResponseModelList)
            {
                byte[] bytes = wc.DownloadData(item.jetpack_featured_media_url);
                MemoryStream ms = new MemoryStream(bytes);
                System.Drawing.Image original = System.Drawing.Image.FromStream(ms);
                _amazonS3FileProvider.UploadBlogImage(original, item.id);

            }
        }
    }
}