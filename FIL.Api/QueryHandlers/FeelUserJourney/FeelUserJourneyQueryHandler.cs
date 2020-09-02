using FIL.Api.Providers;
using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.FeelUserJourney;
using FIL.Contracts.QueryResults.FeelUserJourney;
using FIL.Logging;
using FIL.Logging.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.FeelUserJourney
{
    public class FeelUserJourneyQueryHandler : IQueryHandler<FeelUserJourneyQuery, FeelUserJourneyQueryResult>
    {
        private IEventCategoryMappingRepository _eventCategoryMappingRepository;
        private IEventCategoryRepository _eventCategory;
        private IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private ICountryDescriptionRepository _countryDescriptionRepository;
        private IStateDescriptionRepository _stateDescriptionRepository;
        private ICountryRepository _countryRepository;
        private ICityDescriptionRepository _cityDescriptionRepository;
        private ISubCategoryProvider _subCategoryProvider;
        private IPlaceProvider _placeProvider;
        private IDynamicSectionProvider _dynamicSectionProvider;
        private readonly ILogger _logger;

        public FeelUserJourneyQueryHandler(
            IEventCategoryMappingRepository eventCategoryMappingRepository,
            IEventCategoryRepository eventCategory,
            ICountryRepository countryRepository,
            ICountryDescriptionRepository countryDescriptionRepository,
            ICityDescriptionRepository cityDescriptionRepository,
            IStateDescriptionRepository stateDescriptionRepository,
            ISubCategoryProvider subCategoryProvider,
            IPlaceProvider placeProvider,
            IDynamicSectionProvider dynamicSectionProvider,
            ILogger logger,
            IEventTicketAttributeRepository eventTicketAttributeRepository
            )
        {
            _eventCategoryMappingRepository = eventCategoryMappingRepository;
            _eventCategory = eventCategory;
            _logger = logger;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _countryDescriptionRepository = countryDescriptionRepository;
            _cityDescriptionRepository = cityDescriptionRepository;
            _countryRepository = countryRepository;
            _subCategoryProvider = subCategoryProvider;
            _placeProvider = placeProvider;
            _dynamicSectionProvider = dynamicSectionProvider;
            _stateDescriptionRepository = stateDescriptionRepository;
        }

        public GeoLocation GeoLocation(string Name, int Id, string Slug, string subcatSlug,
            bool isCategoryLevel, int EventCategoryId, int SubCategoryId, int countryId,
            string queryStringParameter, PageDetail pageDetail)
        {
            GeoLocation geoLocation = new GeoLocation();
            geoLocation.Name = Name;
            geoLocation.Id = Id;
            if (pageDetail.PageType == Contracts.Enums.PageType.Category)
            {
                geoLocation.Url = "/c/" + Slug + "/" + (isCategoryLevel ? "" : subcatSlug + "/") + Name.Replace(" ", "-").Replace(".", "").ToLower();
                geoLocation.Query = isCategoryLevel ? "?category=" + EventCategoryId + "&" + queryStringParameter + "=" + Id : "?category=" + EventCategoryId + "&subcategory=" + SubCategoryId + "&" + queryStringParameter + "=" + Id;
            }
            else
            {
                geoLocation.Url = pageDetail.PagePath + "/" + Name.Replace(" ", "-").Replace(".", "").ToLower();
                geoLocation.Query = "?country=" + countryId + ((!pageDetail.IsCategoryLevel && !pageDetail.IsSubCategoryLevel) ? "&" + queryStringParameter + "=" + Id : isCategoryLevel ? "&category=" + EventCategoryId + "&" + queryStringParameter + "=" + Id : "&category=" + EventCategoryId + "&subcategory=" + SubCategoryId + "&" + queryStringParameter + "=" + Id);
            }
            return geoLocation;
        }

        public FeelUserJourneyQueryResult Handle(FeelUserJourneyQuery query)
        {
            try
            {
                var isCategoryLevel = query.CategoryId != 0;
                var isSubcategoryLevel = query.SubCategoryId != 0;
                var isCountryLevel = query.CountryId != 0;
                var isStateLevel = query.StateId != 0;
                var isCityLevel = query.CityId != 0;
                var isOnlineExperiences = false;
                var EventCategory = query.MasterEventType == Contracts.Enums.MasterEventType.Online ? _eventCategory.GetAll().Where(s => s.MasterEventTypeId == Contracts.Enums.MasterEventType.Online).FirstOrDefault() : _eventCategory.Get(query.CategoryId);
                var allCategories = new List<FIL.Contracts.DataModels.EventCategory>();
                if (EventCategory == null && query.PageType == Contracts.Enums.PageType.Category)
                {
                    return new FeelUserJourneyQueryResult { };
                }
                if (!isCountryLevel && query.PageType == Contracts.Enums.PageType.Country)
                {
                    return new FeelUserJourneyQueryResult { };
                }
                if (isCountryLevel && query.PageType == Contracts.Enums.PageType.Country)
                {
                    if (isCategoryLevel && query.CategoryId == 0) // If Global country/state/city page
                    {
                        isCategoryLevel = false;
                    }
                    if (isSubcategoryLevel && query.SubCategoryId == 0) // If Global country/state/city page
                    {
                        isSubcategoryLevel = false;
                    }
                    var country = _countryRepository.Get(query.CountryId);
                    if (country == null)
                    {
                        return new FeelUserJourneyQueryResult { };
                    }
                }
                var subCategories = _eventCategory.GetByEventCategoryId(query.CategoryId).Where(s => s.IsEnabled).ToList();
                if (query.MasterEventType == Contracts.Enums.MasterEventType.Online || query.MasterEventType == Contracts.Enums.MasterEventType.InRealLife)
                {
                    isOnlineExperiences = query.MasterEventType == Contracts.Enums.MasterEventType.Online ? true : false;
                    allCategories = _eventCategory.GetAll().ToList();
                    var parentCategoryIds = allCategories.Where(s => s.MasterEventTypeId == query.MasterEventType).Select(s => s.Id);
                    subCategories = allCategories.Where(s => parentCategoryIds.Any(p => p == s.EventCategoryId)).ToList();
                }
                if (isSubcategoryLevel)
                {
                    subCategories = subCategories.Where(s => s.Id == query.SubCategoryId).ToList();
                }
                var firstEventCategory = _eventCategory.Get(subCategories.FirstOrDefault().EventCategoryId);
                if (firstEventCategory != null && (!isCountryLevel && !isStateLevel && !isCategoryLevel && !isCityLevel && !isSubcategoryLevel))
                {
                    isOnlineExperiences = firstEventCategory.MasterEventTypeId == Contracts.Enums.MasterEventType.Online ? true : false;
                }
                var placeDetails = _placeProvider.GetAllPlaces(query, subCategories, EventCategory, query.MasterEventType, isCountryLevel, isStateLevel, isCityLevel).Where(s => s.CurrencyId != 0 && !s.IsTokenize).ToList();
                if (!placeDetails.Any())
                {
                    return new FeelUserJourneyQueryResult { };
                }
                var pageDetail = new PageDetail
                {
                    PageType = query.PageType,
                    PagePath = query.PagePath,
                    IsCategoryLevel = isCategoryLevel,
                    IsSubCategoryLevel = isSubcategoryLevel,
                    IsCountryLevel = isCountryLevel,
                    IsStateLevel = isStateLevel,
                    IsCityLevel = isCityLevel
                };
                List<DynamicPlaceSections> DynamicPlaceSections = new List<DynamicPlaceSections>();
                CountryPageDetail countryPageDetail = new CountryPageDetail();
                DynamicPlaceSections AllplaceTiles = new DynamicPlaceSections();
                List<GeoLocation> Cities = new List<GeoLocation>();
                List<GeoLocation> States = new List<GeoLocation>();
                List<GeoLocation> Countries = new List<GeoLocation>();
                List<SubCategory> allSubCategories = new List<SubCategory>();

                if (((!isCityLevel || isCategoryLevel) && query.PageType == Contracts.Enums.PageType.Category) ||
                    (query.PageType == Contracts.Enums.PageType.Country && (!isSubcategoryLevel && !isCityLevel))
                    ) // Get subcat for category => category/country/state OR Country => category/country/state
                {
                    if (query.MasterEventType == Contracts.Enums.MasterEventType.Online || query.MasterEventType == Contracts.Enums.MasterEventType.InRealLife)
                    {
                        subCategories = allCategories.Where(s => s.MasterEventTypeId == query.MasterEventType).ToList();
                    }
                    if (((isCountryLevel || isStateLevel || isCityLevel) && query.PageType == Contracts.Enums.PageType.Category) ||
                        ((isStateLevel || isCityLevel || isCountryLevel) && (isCategoryLevel) && query.PageType == Contracts.Enums.PageType.Category)) // filter the subcat if it's category => state/country or country => category page
                    {
                        subCategories = subCategories.Where(x => placeDetails.Any(y => y.EventCategoryId == x.Id)).ToList();
                    }
                    else if (query.PageType == Contracts.Enums.PageType.Country && !isCategoryLevel && !isSubcategoryLevel) // Global country/city/state landing page
                    {
                        subCategories = _eventCategory.GetByIds(placeDetails.Select(s => s.ParentCategoryId).Distinct()).ToList();
                    }
                    allSubCategories = _subCategoryProvider.GetSubCategories(query, placeDetails, subCategories, pageDetail).OrderBy(s => s.Order).ToList();
                }

                if (((!isCityLevel || isCountryLevel) && pageDetail.PageType == Contracts.Enums.PageType.Category) ||
                    (pageDetail.PageType == Contracts.Enums.PageType.Country && !(isSubcategoryLevel && isCityLevel)))
                {
                    var cityGroup = placeDetails.Select(s => new { s.CityId, s.CityName }).Distinct().Take(10);
                    var stateGroup = placeDetails.Select(s => new { s.StateId, s.StateName }).Distinct().Take(10);
                    // If it's online then don't show city/state/country
                    if (!isOnlineExperiences)
                    {
                        foreach (var group in cityGroup)
                        {
                            GeoLocation city = new GeoLocation();
                            if (group.CityName != "")
                            {
                                Cities.Add(GeoLocation(group.CityName, group.CityId, placeDetails.First().ParentCategorySlug, subCategories.First().Slug, isCategoryLevel, placeDetails.First().ParentCategoryId, subCategories.First().Id, query.CountryId, "city", pageDetail));
                            }
                        }
                        foreach (var group in stateGroup)
                        {
                            GeoLocation state = new GeoLocation();
                            if (group.StateName != "")
                            {
                                States.Add(GeoLocation(group.StateName, group.StateId, placeDetails.First().ParentCategorySlug, subCategories.First().Slug, isCategoryLevel, placeDetails.First().ParentCategoryId, subCategories.First().Id, query.CountryId, "state", pageDetail));
                            }
                        }
                        if (query.PageType == Contracts.Enums.PageType.Category)
                        {
                            var countryGroup = placeDetails.Select(s => new { s.CountryId, s.CountryName }).Distinct().Take(10);
                            foreach (var group in countryGroup)
                            {
                                GeoLocation country = new GeoLocation();
                                if (group.CountryName != "")
                                {
                                    Countries.Add(GeoLocation(group.CountryName, group.CountryId, placeDetails.First().ParentCategorySlug, subCategories.First().Slug, isCategoryLevel, placeDetails.First().ParentCategoryId, subCategories.First().Id, query.CountryId, "country", pageDetail));
                                }
                            }
                        }
                    }
                    DynamicPlaceSections = _dynamicSectionProvider.GetDynamicSections(placeDetails, query.MasterEventType, pageDetail);
                }

                AllplaceTiles.PlaceDetails = placeDetails.Take(320).ToList();
                AllplaceTiles.SectionDetails = new SectionDetail();
                if (query.MasterEventType == Contracts.Enums.MasterEventType.Online || query.MasterEventType == Contracts.Enums.MasterEventType.InRealLife)
                {
                    AllplaceTiles.SectionDetails.Heading = query.MasterEventType == Contracts.Enums.MasterEventType.Online ? "Top Online Experiences from around the World" :
                        "In-Real-Life experiences around the world";
                }
                else if (query.PageType == Contracts.Enums.PageType.Category)
                {
                    AllplaceTiles.SectionDetails.Heading = (isCategoryLevel ? EventCategory.DisplayName : subCategories.First().DisplayName) + (isOnlineExperiences ? " from around " : " around ") +
         (isCountryLevel ? placeDetails.First().CountryName : isStateLevel ?
        placeDetails.First().StateName : isCityLevel ? placeDetails.First().CityName : "the World");
                }
                else if (query.PageType == Contracts.Enums.PageType.Country)
                {
                    AllplaceTiles.SectionDetails.Heading = ((!pageDetail.IsCategoryLevel && !pageDetail.IsSubCategoryLevel) ? "Feels"
                        : pageDetail.IsCategoryLevel
                        ? placeDetails.First().ParentCategory
                        : pageDetail.IsSubCategoryLevel ? placeDetails.First().Category : "") + (isOnlineExperiences ? " from around " : " around ") + (pageDetail.IsCityLevel
                        ? placeDetails.First().CityName :
                        pageDetail.IsStateLevel ? placeDetails.First().StateName : placeDetails.First().CountryName);
                }
                AllplaceTiles.SectionDetails.IsShowMore = true;
                var searchValue = "";
                if (query.MasterEventType == Contracts.Enums.MasterEventType.Online || query.MasterEventType == Contracts.Enums.MasterEventType.InRealLife)
                {
                    searchValue = "";
                }
                else if (query.PageType == Contracts.Enums.PageType.Category)
                {
                    searchValue = placeDetails.First().ParentCategory + (isSubcategoryLevel ? ", " + placeDetails.First().Category : "") +
                        (isCountryLevel ? ", " + placeDetails.First().CountryName : "") + (isStateLevel ? ", " + placeDetails.First().StateName : "") +
                        (isCityLevel ? ", " + placeDetails.First().CityName : "");
                }
                else
                {
                    searchValue = placeDetails.First().CountryName + (isSubcategoryLevel ? ", " + placeDetails.First().Category : "" + (isCategoryLevel ? ", " + placeDetails.First().ParentCategory : "")) +
                        (isStateLevel ? ", " + placeDetails.First().StateName : "") +
                        (isCityLevel ? ", " + placeDetails.First().CityName : "");
                }
                if (query.PageType == Contracts.Enums.PageType.Country)
                {
                    DynamicPlaceSections = DynamicPlaceSections.OrderBy(a => Guid.NewGuid()).ToList();
                    var description = "";
                    if (pageDetail.IsCityLevel)
                    {
                        var cityDescription = _cityDescriptionRepository.GetBycityId(placeDetails.First().CityId);
                        description = cityDescription != null ? cityDescription.Description : "";
                    }
                    else if (pageDetail.IsStateLevel)
                    {
                        var stateDescription = _stateDescriptionRepository.GetByStateId(placeDetails.First().StateId);
                        description = stateDescription != null ? stateDescription.Description : "";
                    }
                    else
                    {
                        var countryDescription = _countryDescriptionRepository.GetByCountryId(query.CountryId);
                        description = countryDescription != null ? countryDescription.Description : "";
                    }
                    countryPageDetail.Count = (pageDetail.IsCountryLevel && !pageDetail.IsStateLevel && !pageDetail.IsCityLevel) ? _countryRepository.GetAllCountryPlace().Where(s => s.Id == query.CountryId).FirstOrDefault().Count : placeDetails.GroupBy(x => x.Name, (key, group) => group.First()).Count();
                    countryPageDetail.Description = description;
                    countryPageDetail.SectionTitle = pageDetail.IsCityLevel ? placeDetails.First().CityName : isStateLevel ? placeDetails.First().StateName :
                        placeDetails.First().CountryName;
                }

                return new FeelUserJourneyQueryResult
                {
                    AllPlaceTiles = AllplaceTiles,
                    SubCategories = allSubCategories,
                    GeoData = new GeoData { Cities = Cities, Countries = Countries, States = States },
                    DynamicPlaceSections = DynamicPlaceSections,
                    SearchValue = searchValue,
                    ContryPageDetails = countryPageDetail,
                    Success = true
                };
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, new Exception("", e));
                return new FeelUserJourneyQueryResult
                {
                };
            }
        }
    }
}