using FIL.Api.Repositories;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Contracts.Queries.EventCreation;
using FIL.Contracts.QueryResults.EventCreation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.EventCreation
{
    public class SavedEventQueryHandler : IQueryHandler<SavedEventQuery, SavedEventQueryResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventCategoryMappingRepository _eventCategoryMappingRepository;
        private readonly IEventSiteIdMappingRepository _eventSiteIdMappingRepository;
        private readonly IEventLearnMoreAttributeRepository _eventLearnMoreAttributeRepository;
        private readonly IEventAmenityRepository _eventAmenityRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventGalleryImageRepository _eventGalleryImageRepository;
        private readonly IEventCategoryRepository _eventCategoryRepository;
        private readonly IZipcodeRepository _zipcodeRepository;
        private readonly IPlaceVisitDurationRepository _placeVisitDurationRepository;
        private readonly IEventTagMappingRepository _eventTagMappingRepository;
        private readonly IEventHostMappingRepository _eventHostMappingRepository;

        public SavedEventQueryHandler(IEventRepository eventRepository, IEventCategoryMappingRepository eventCategoryMappingRepository,
            IEventSiteIdMappingRepository eventSiteIdMappingRepository, IEventLearnMoreAttributeRepository eventLearnMoreAttributeRepository,
            IEventTagMappingRepository eventTagMappingRepository,
            IEventHostMappingRepository eventHostMappingRepository,
            ICountryRepository countryRepository, IZipcodeRepository zipcodeRepository, IPlaceVisitDurationRepository placeVisitDurationRepository, IStateRepository stateRepository, IEventAmenityRepository eventAmenityRepository,
            ICityRepository cityRepository, IEventCategoryRepository eventCategoryRepository, IVenueRepository venueRepository, IEventDetailRepository eventDetailRepository, IMediator mediator, IEventGalleryImageRepository eventGalleryImageRepository)
        {
            _eventRepository = eventRepository;
            _eventCategoryMappingRepository = eventCategoryMappingRepository;
            _eventSiteIdMappingRepository = eventSiteIdMappingRepository;
            _eventLearnMoreAttributeRepository = eventLearnMoreAttributeRepository;
            _eventGalleryImageRepository = eventGalleryImageRepository;

            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
            _cityRepository = cityRepository;
            _venueRepository = venueRepository;
            _eventDetailRepository = eventDetailRepository;
            _eventAmenityRepository = eventAmenityRepository;
            _eventCategoryRepository = eventCategoryRepository;
            _zipcodeRepository = zipcodeRepository;
            _placeVisitDurationRepository = placeVisitDurationRepository;
            _eventTagMappingRepository = eventTagMappingRepository;
            _eventHostMappingRepository = eventHostMappingRepository;
        }

        public SavedEventQueryResult Handle(SavedEventQuery query)
        {
            var eventDataModel = _eventRepository.GetByAltId(query.Id);
            var eventModel = AutoMapper.Mapper.Map<Contracts.Models.Event>(eventDataModel);

            var placeVisitDuration = _placeVisitDurationRepository.GetBySingleEventId(eventModel.Id);

            var galleryImageResponse = _eventGalleryImageRepository.GetByEventId(eventModel.Id);
            var galleryImageModel = AutoMapper.Mapper.Map<IEnumerable<EventGalleryImage>>(galleryImageResponse);

            var eventcatmapresponse = _eventCategoryMappingRepository.GetByEventId(eventModel.Id);
            var eventcatmap = AutoMapper.Mapper.Map<IEnumerable<EventCategoryMapping>>(eventcatmapresponse);

            var eventsiteidresponse = _eventSiteIdMappingRepository.GetByEventId(eventModel.Id);
            var eventsitemap = AutoMapper.Mapper.Map<EventSiteIdMapping>(eventsiteidresponse);

            var eventlearnmoreresponse = _eventLearnMoreAttributeRepository.GetByEventId(eventModel.Id);
            var eventlearnmore = AutoMapper.Mapper.Map<IEnumerable<EventLearnMoreAttribute>>(eventlearnmoreresponse);

            var eventamenityresponse = _eventAmenityRepository.GetByEventId(eventModel.Id);
            var eventamenity = AutoMapper.Mapper.Map<IEnumerable<EventAmenity>>(eventamenityresponse);

            var eventdetailresponse = _eventDetailRepository.GetSubeventByEventId(eventModel.Id).FirstOrDefault();
            var eventdetail = AutoMapper.Mapper.Map<EventDetail>(eventdetailresponse);

            var eventTags = _eventTagMappingRepository.GetByEventId(eventModel.Id);

            Country countrydetail = new Country();
            City citydetail = new City();
            State statedetail = new State();
            Zipcode zipcodeDetail = new Zipcode();
            Venue venuedetail = new Venue();
            if (eventdetail != null)
            {
                var venuedetailresponse = _venueRepository.GetByVenueId(eventdetail.VenueId);
                venuedetail = AutoMapper.Mapper.Map<Venue>(venuedetailresponse);
                if (venuedetail != null)
                {
                    var citydetailresponse = _cityRepository.GetByCityId(venuedetail.CityId);
                    citydetail = AutoMapper.Mapper.Map<City>(citydetailresponse);
                    var Zip = _zipcodeRepository.GetAllByCityId(citydetail.Id).FirstOrDefault();
                    zipcodeDetail = AutoMapper.Mapper.Map<Zipcode>(Zip);
                }
                if (citydetail != null)
                {
                    var statedetailresponse = _stateRepository.GetByStateId(citydetail.StateId);
                    statedetail = AutoMapper.Mapper.Map<State>(statedetailresponse);
                }
                if (statedetail != null)
                {
                    var countrydetailresponse = _countryRepository.GetByCountryId(statedetail.CountryId);
                    countrydetail = AutoMapper.Mapper.Map<Country>(countrydetailresponse);
                }
            }

            var features = Enum.GetValues(typeof(LearnMoreFeatures));
            var resultdata = new SavedEventQueryResult();

            resultdata.Country = countrydetail.Name;
            resultdata.City = citydetail.Name;
            resultdata.Address1 = venuedetail.AddressLineOne;
            resultdata.Address2 = venuedetail.AddressLineTwo;
            resultdata.State = statedetail.Name;
            resultdata.Lat = ((venuedetail.Latitude != null && venuedetail.Latitude != "" && venuedetail.Latitude != "NaN") ? venuedetail.Latitude : "18.5204");
            resultdata.Long = ((venuedetail.Longitude != null && venuedetail.Longitude != "" && venuedetail.Latitude != "NaN") ? venuedetail.Longitude : "73.8567");
            List<string> amenityids = new List<string>();
            if (zipcodeDetail != null)
            {
                if (zipcodeDetail.Id != 0)
                {
                    resultdata.Zip = zipcodeDetail.Postalcode;
                }
            }
            foreach (var ea in eventamenity)
            {
                amenityids.Add((ea.AmenityId).ToString());
            }
            if (placeVisitDuration != null)
            {
                var data = placeVisitDuration.TimeDuration.Split("-");
                if (data.Length >= 2)
                {
                    resultdata.HourTimeDuration = placeVisitDuration.TimeDuration;
                    resultdata.MinuteTimeDuration = "";
                }
            }
            resultdata.AmenityId = string.Join(",", amenityids);
            //resultdata.AmenityId = string.Join(",", amenityids);
            var archdesc = eventlearnmore.FirstOrDefault(p => p.LearnMoreFeatureId == LearnMoreFeatures.ArchitecturalDetail);
            if (archdesc != null)
            {
                resultdata.Archdetail = archdesc.Description;
                resultdata.ArchdetailId = archdesc.Id;
            }
            var highlightdesc = eventlearnmore.FirstOrDefault(p => p.LearnMoreFeatureId == LearnMoreFeatures.HighlightNugget);
            if (highlightdesc != null)
            {
                resultdata.Highlights = highlightdesc.Description;
                resultdata.HighlightsId = highlightdesc.Id;
            }
            var historydesc = eventlearnmore.FirstOrDefault(p => p.LearnMoreFeatureId == LearnMoreFeatures.History);
            if (historydesc != null)
            {
                resultdata.History = historydesc.Description;
                resultdata.HistoryId = historydesc.Id;
            }
            var immersdesc = eventlearnmore.FirstOrDefault(p => p.LearnMoreFeatureId == LearnMoreFeatures.ImmersiveExperience);
            if (immersdesc != null)
            {
                resultdata.Immersiveexperience = immersdesc.Description;
                resultdata.ImmersiveexperienceId = immersdesc.Id;
            }

            var tilesImages = "";
            var galleryImages = galleryImageModel.Where(p => p.Name.Contains("gallery"));
            var tilesSliderImages = galleryImageModel.Where(p => p.Name.Contains("Tiles"));
            var descPageImages = galleryImageModel.Where(p => p.Name.Contains("DescBanner"));
            var inventoryPageImages = galleryImageModel.Where(p => p.Name.Contains("InventoryBanner"));
            var placeMapImages = galleryImageModel.Where(p => p.Name.Contains("placemapImages"));
            var timelineImages = galleryImageModel.Where(p => p.Name.Contains("timelineImages"));
            var immersiveImages = galleryImageModel.Where(p => p.Name.Contains("immersiveImages"));
            var architecturalImages = galleryImageModel.Where(p => p.Name.Contains("archdetailImages"));

            var galleryImagesList = "";
            var tilesSliderImagesList = "";
            var descPageList = "";
            var inventoryPageList = "";
            var plaeMapImagesList = "";
            var timelineImagesList = "";
            var immerseImagesList = "";
            var archImagesList = "";

            foreach (EventGalleryImage eventGalleryImage in galleryImages) { galleryImagesList += eventGalleryImage.Name + ","; }
            foreach (EventGalleryImage eventGalleryImage in tilesSliderImages) { tilesSliderImagesList += eventGalleryImage.Name + ","; }
            foreach (EventGalleryImage eventGalleryImage in descPageImages) { descPageList += eventGalleryImage.Name + ","; }
            foreach (EventGalleryImage eventGalleryImage in inventoryPageImages) { inventoryPageList += eventGalleryImage.Name + ","; }
            foreach (EventGalleryImage eventGalleryImage in placeMapImages) { plaeMapImagesList += eventGalleryImage.Name + ","; }
            foreach (EventGalleryImage eventGalleryImage in timelineImages) { timelineImagesList += eventGalleryImage.Name + ","; }
            foreach (EventGalleryImage eventGalleryImage in immersiveImages) { immerseImagesList += eventGalleryImage.Name + ","; }
            foreach (EventGalleryImage eventGalleryImage in galleryImages) { archImagesList += eventGalleryImage.Name + ","; }

            resultdata.GalleryImages = galleryImagesList;
            resultdata.TilesSliderImages = tilesSliderImagesList;
            resultdata.DescpagebannerImage = descPageList;
            resultdata.InventorypagebannerImage = inventoryPageList;
            resultdata.PlacemapImages = plaeMapImagesList;
            resultdata.TimelineImages = timelineImagesList;
            resultdata.ImmersiveexpImages = immerseImagesList;
            resultdata.ArchdetailImages = archImagesList;

            resultdata.Description = eventModel.Description;
            resultdata.Id = eventModel.Id;
            resultdata.AltId = eventModel.AltId;
            resultdata.Location = eventModel.Name;
            List<int> subcatids = new List<int>();
            List<int> tags = new List<int>();
            int categoryId = eventModel.EventCategoryId;
            foreach (var cat in eventcatmap)
            {
                categoryId = cat.EventCategoryId;
                subcatids.Add(cat.EventCategoryId);
            }
            foreach (var tag in eventTags)
            {
                tags.Add((int)tag.TagId);
            }
            var category = _eventCategoryRepository.Get(categoryId);
            resultdata.Subcategoryid = string.Join(",", subcatids);
            resultdata.TagIds = string.Join(",", tags);
            resultdata.Categoryid = category.EventCategoryId;
            //resultdata.Subcategoryid = subcatids.FirstOrDefault();
            /*var categorymapobj = eventcatmap.FirstOrDefault();
            if (categorymapobj != null)
            {
                resultdata.Categoryid = eventModel.EventCategoryId;
            }*/
            resultdata.Metadescription = eventModel.MetaDetails;
            if (resultdata.Metadescription != null)
            {
                string[] metas = resultdata.Metadescription.Split(new string[] { "<br/>" }, StringSplitOptions.None);
                if (metas.Length == 3)
                {
                    resultdata.Metatitle = metas[0].Split("title")[1].Replace(">", "").Replace("</", "");
                    resultdata.Metadescription = metas[1].Split("content=")[1].Replace(">", "").Replace("\"", "").Replace("</", "");
                    resultdata.Metatags = metas[2].Split("content=")[1].Replace(">", "").Replace("\"", "").Replace("</", "");
                }
            }
            resultdata.PlaceName = venuedetail.Name;
            //resultdata.Subcategoryid = eventcatmap.Select(p => p.EventCategoryId).FirstOrDefault();
            //resultdata.Subcategoryid = string.Join(",", eventcatmap.Count() > 0 ? eventcatmap.Select(p => p.EventCategoryId).ToList() : new List<int>());
            resultdata.Title = eventModel.Name;
            var eventHostMappings = _eventHostMappingRepository.GetAllByEventId(eventModel.Id);
            resultdata.EventHostMappings = eventHostMappings.ToList();
            return resultdata;
        }
    }
}