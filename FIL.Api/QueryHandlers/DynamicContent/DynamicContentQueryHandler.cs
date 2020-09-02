using FIL.Api.Repositories;
using FIL.Contracts.Models.DynamicContent;
using FIL.Contracts.Queries;
using FIL.Contracts.QueryResults.DyanamicContent;
using FIL.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.DynamicContent
{
    public class DynamicContentQueryHandler : IQueryHandler<DynamicContentQuery, DynamicContentQueryResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IEventCategoryMappingRepository _eventCategoryMappingRepository;
        private readonly IEventCategoryRepository _eventCategoryRepository;
        private readonly ILogger _logger;

        public DynamicContentQueryHandler(IEventRepository eventRepository, IEventDetailRepository eventDetailRepository, IVenueRepository venueRepository, ICityRepository cityRepository, IStateRepository stateRepository, ICountryRepository countryRepository, IEventCategoryMappingRepository eventCategoryMappingRepository, IEventCategoryRepository eventCategoryRepository, ILogger logger)
        {
            _eventRepository = eventRepository;
            _eventDetailRepository = eventDetailRepository;
            _venueRepository = venueRepository;
            _cityRepository = cityRepository;
            _stateRepository = stateRepository;
            _countryRepository = countryRepository;
            _eventCategoryMappingRepository = eventCategoryMappingRepository;
            _eventCategoryRepository = eventCategoryRepository;
            _logger = logger;
        }

        public DynamicContentQueryResult Handle(DynamicContentQuery query)
        {
            DynamicContentResponseModel dynamicContentData = new DynamicContentResponseModel();
            try
            {
                //Get data for the Place Page
                if (query.Url.Contains("/place/"))
                {
                    var slug = query.Url.Split("/");
                    if (slug.Length >= 5)
                    {
                        var eventModelData = _eventRepository.GetAllBySlug(slug[5]).FirstOrDefault();
                        if (eventModelData != null)
                        {
                            dynamicContentData.PlaceName = eventModelData.Name;
                            dynamicContentData.VenueName = eventModelData.Venue;
                            dynamicContentData.CityName = eventModelData.CityName;
                            dynamicContentData.StateName = eventModelData.StateName;
                            dynamicContentData.CountryName = eventModelData.CountryName;
                            dynamicContentData.ParentCategoryName = eventModelData.ParentCategory;
                            dynamicContentData.ParentCategoryUrl = "/c/" + eventModelData.ParentCategorySlug + "?category=" + eventModelData.ParentCategoryId;
                            dynamicContentData.SubCategoryName = eventModelData.Category;
                            dynamicContentData.SubCategoryUrl = "/c/" + eventModelData.ParentCategorySlug + "/" + eventModelData.SubCategorySlug + "?category=" + eventModelData.ParentCategoryId + "subcategory=" + eventModelData.EventCategoryId;
                            dynamicContentData.PlaceDescription = eventModelData.EventDescription;
                            dynamicContentData.AltId = eventModelData.AltId;
                            //Get Data required for product markup
                            var ticketDetailsList = _eventDetailRepository.GetAllTicketDetails(eventModelData.EventDetailId);
                            List<ProductMarkup> productMarkupModelList = new List<ProductMarkup>();

                            ProductMarkup productMarkupModel = new ProductMarkup();
                            productMarkupModel.@type = "AggregateOffer";
                            productMarkupModel.availabilityEnds = eventModelData.EventEndDateTime;
                            productMarkupModel.availabilityStarts = eventModelData.EventStartDateTime;
                            productMarkupModel.lowPrice = eventModelData.MinPrice;
                            productMarkupModel.highPrice = eventModelData.MaxPrice;
                            productMarkupModel.priceCurrency = eventModelData.Currency;
                            productMarkupModel.validFrom = eventModelData.EventStartDateTime;
                            productMarkupModel.availability = "http://schema.org/InStock";
                            productMarkupModel.url = query.Url;
                            productMarkupModel.name = null;
                            productMarkupModel.price = eventModelData.MinPrice;
                            productMarkupModelList.Add(productMarkupModel);

                            foreach (var currentTicketDetail in ticketDetailsList)
                            {
                                ProductMarkup currentMarkupModel = new ProductMarkup();
                                currentMarkupModel.@type = "Offer";
                                currentMarkupModel.availabilityEnds = currentTicketDetail.SalesEndDateTime;
                                currentMarkupModel.availabilityStarts = currentTicketDetail.SalesStartDateTime;
                                currentMarkupModel.priceCurrency = currentTicketDetail.Currency;
                                currentMarkupModel.validFrom = currentTicketDetail.SalesStartDateTime;
                                currentMarkupModel.availability = "http://schema.org/InStock";
                                currentMarkupModel.name = currentTicketDetail.Name;
                                currentMarkupModel.url = query.Url;
                                currentMarkupModel.price = currentTicketDetail.Price;
                                currentMarkupModel.lowPrice = null;
                                currentMarkupModel.highPrice = null;
                                productMarkupModelList.Add(currentMarkupModel);
                            }
                            dynamicContentData.MasterEventTypeId = eventModelData.MasterEventTypeId;
                            dynamicContentData.ProductMarkupModelList = productMarkupModelList;
                        }
                    }
                }
                else
                {
                    var content = GetContent(query.Url, query.QueryString);

                    dynamicContentData.CityName = content.CityName;
                    dynamicContentData.StateName = content.StateName;
                    dynamicContentData.CountryName = content.CountryName;
                    dynamicContentData.ParentCategoryName = content.CategoryName;
                    dynamicContentData.SubCategoryName = content.SubCategoryName;
                    dynamicContentData.GenericLocationName = content.LocationName;
                }
            }
            catch (Exception e)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, new Exception("Exeception Caught at Dynamic Source Provider " + e.Message, e));
            }
            return new DynamicContentQueryResult
            {
                DynamicContentResponseModel = dynamicContentData
            };
        }

        private ContentModel GetContent(string url, string queryString)
        {
            ContentModel ContentModel = new ContentModel();

            if (url.Contains("country") && !url.Contains("/c/"))
            {
                if (queryString.Contains("country") && !queryString.Contains("state") && !queryString.Contains("city"))
                {
                    int countryId = new int();
                    if (queryString.Split("=").Length >= 1 && int.TryParse(queryString.Split("=")[1].Replace("&category", ""), out countryId))
                    {
                        var countryModel = _countryRepository.Get(countryId);
                        ContentModel.LocationName = countryModel.CountryName;
                    }
                }
                else if (queryString.Contains("state"))
                {
                    int stateId = new int();
                    if (queryString.Split("state").Length >= 1 && int.TryParse(queryString.Split("state")[1].Replace("=", ""), out stateId))
                    {
                        var stateModel = _stateRepository.Get(stateId);
                        ContentModel.LocationName = stateModel.Name;
                    }
                }
                else if (queryString.Contains("city"))
                {
                    int cityId = new int();
                    if (queryString.Split("city").Length >= 1 && int.TryParse(queryString.Split("city")[1].Replace("=", ""), out cityId))
                    {
                        var cityModel = _cityRepository.Get(cityId);
                        ContentModel.LocationName = cityModel.Name;
                    }
                }
            }
            //Get data for the Category Related Pages
            else if (queryString.Contains("category"))
            {
                if (queryString.Contains("category") && !queryString.Contains("subcategory") && (!queryString.Contains("country") && !queryString.Contains("state") && !queryString.Contains("city")))
                {
                    int categoryId = new int();
                    if (queryString.Split("category").Length > 1 && int.TryParse(queryString.Split("category")[1].Replace("=", "").Replace("sub", "").Replace("&", ""), out categoryId))
                    {
                        var categoryModel = _eventCategoryRepository.Get(categoryId);
                        ContentModel.CategoryName = categoryModel.DisplayName;
                    }
                }

                if (queryString.Contains("subcategory"))
                {
                    int categoryId = new int();
                    if (queryString.Split("category").Length >= 1 && int.TryParse(queryString.Split("category")[1].Replace("=", "").Replace("sub", "").Replace("&", ""), out categoryId))
                    {
                        var categoryModel = _eventCategoryRepository.Get(categoryId);
                        ContentModel.CategoryName = categoryModel.DisplayName;
                    }
                }

                if (queryString.Contains("category") && !queryString.Contains("subcategory") && (queryString.Contains("country") || queryString.Contains("state") || queryString.Contains("city")))
                {
                    int categoryId = new int();
                    if (queryString.Split("&").Length >= 0 && int.TryParse(queryString.Split("&")[0].Replace("=", "").Replace("country", "").Replace("city", "").Replace("state", "").Replace("?category", ""), out categoryId))
                    {
                        var categoryModel = _eventCategoryRepository.Get(categoryId);
                        ContentModel.CategoryName = categoryModel.DisplayName;
                    }
                }
                if (queryString.Contains("subcategory") && !queryString.Contains("country") && !queryString.Contains("state") && !queryString.Contains("city"))
                {
                    int subCategoryId = new int();
                    if (queryString.Split("subcategory").Length >= 1 && int.TryParse(queryString.Split("subcategory")[1].Replace("=", ""), out subCategoryId))
                    {
                        var subCategoryModel = _eventCategoryRepository.Get(subCategoryId);
                        ContentModel.SubCategoryName = subCategoryModel.DisplayName;
                    }
                }
                if ((queryString.Contains("subcategory")) && (queryString.Contains("country") || queryString.Contains("state") || queryString.Contains("city")))
                {
                    int subCategoryId = new int();
                    if (queryString.Split("&").Length >= 1 && int.TryParse(queryString.Split("&")[1].Replace("=", "").Replace("subcategory", ""), out subCategoryId))
                    {
                        var subCategoryModel = _eventCategoryRepository.Get(subCategoryId);
                        ContentModel.SubCategoryName = subCategoryModel.DisplayName;
                    }
                }
                if (queryString.Contains("country"))
                {
                    int countryId = new int();
                    if (queryString.Split("country").Length >= 1 && int.TryParse(queryString.Split("country")[1].Replace("=", ""), out countryId))
                    {
                        var countryModel = _countryRepository.Get(countryId);
                        ContentModel.CountryName = countryModel.CountryName;
                    }
                }
                if (queryString.Contains("state"))
                {
                    int stateId = new int();
                    if (queryString.Split("state").Length >= 1 && int.TryParse(queryString.Split("state")[1].Replace("=", ""), out stateId))
                    {
                        var stateModel = _stateRepository.Get(stateId);
                        var countryModel = _countryRepository.Get(stateModel.CountryId);
                        ContentModel.StateName = stateModel.Name;
                        ContentModel.CountryName = countryModel.CountryName;
                    }
                }
                if (queryString.Contains("city"))
                {
                    int cityId = new int();
                    if (queryString.Split("city").Length >= 1 && int.TryParse(queryString.Split("city")[1].Replace("=", ""), out cityId))
                    {
                        var cityModel = _cityRepository.Get(cityId);
                        var stateModel = _stateRepository.Get(cityModel.StateId);
                        var countryModel = _countryRepository.Get(stateModel.CountryId);
                        ContentModel.CityName = cityModel.Name;
                        ContentModel.CountryName = countryModel.CountryName;
                    }
                }
            }
            return ContentModel;
        }

        public class ContentModel
        {
            public string LocationName { get; set; }
            public string SubCategoryName { get; set; }
            public string CategoryName { get; set; }
            public string CountryName { get; set; }
            public string CityName { get; set; }
            public string StateName { get; set; }
        }
    }
}