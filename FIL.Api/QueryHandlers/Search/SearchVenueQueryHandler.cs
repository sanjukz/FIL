using FIL.Api.Repositories;
using FIL.Contracts.DataModels;
using FIL.Contracts.Queries.Venue;
using FIL.Contracts.QueryResults;
using FIL.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.Search
{
    public class SearchVenueQueryHandler : IQueryHandler<SearchVenueQuery, SearchVenueQueryResult>
    {
        private readonly ICityRepository _cityRepository;
        private readonly IPlaceVisitDurationRepository _PlaceVisitDurationRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IEventCategoryMappingRepository _eventCategoryMappingRepository;
        private readonly IEventCategoryRepository _eventCategoryRepository;
        private readonly IPlaceWeekOffRepository _placeWeekOffRepository;
        private readonly IPlaceHolidayDatesRepository _placeHolidayDatesRepository;
        private readonly FIL.Logging.ILogger _logger;
        private readonly IPlaceWeekOpenDaysRepository _placeWeekOpenDaysRepository;
        private readonly IDayTimeMappingsRepository _dayTimeMappingsRepository;
        private readonly ISeasonDaysMappingsRepository _seasonDaysMappingsRepository;
        private readonly ISeasonDayTimeMappingsRepository _seasonDayTimeMappingsRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IDaysRepository _daysRepository;

        public SearchVenueQueryHandler(ILogger logger, ICityRepository cityRepository, IEventRepository eventRepository,
            IPlaceVisitDurationRepository PlaceVisitDurationRepository,
             IEventCategoryMappingRepository eventCategoryMappingRepository,
             IEventCategoryRepository eventCategoryRepository,
              IPlaceWeekOpenDaysRepository placeWeekOpenDaysRepository,
            IDayTimeMappingsRepository dayTimeMappingsRepository,
            ISeasonDaysMappingsRepository seasonDaysMappingsRepository,
            ISeasonDayTimeMappingsRepository seasonDayTimeMappingsRepository,
            IDaysRepository daysRepository,
             IEventDetailRepository eventDetailRepository,
        IEventTicketDetailRepository eventTicketDetailRepository,
        IEventTicketAttributeRepository eventTicketAttributeRepository,
        ITicketCategoryRepository ticketCategoryRepository,
        IPlaceWeekOffRepository placeWeekOffRepository, IPlaceHolidayDatesRepository placeHolidayDatesRepository)
        {
            _cityRepository = cityRepository;
            _PlaceVisitDurationRepository = PlaceVisitDurationRepository;
            _eventRepository = eventRepository;
            _eventCategoryMappingRepository = eventCategoryMappingRepository;
            _eventCategoryRepository = eventCategoryRepository;
            _placeWeekOpenDaysRepository = placeWeekOpenDaysRepository;
            _dayTimeMappingsRepository = dayTimeMappingsRepository;
            _logger = logger;
            _placeWeekOffRepository = placeWeekOffRepository;
            _placeHolidayDatesRepository = placeHolidayDatesRepository;
            _seasonDaysMappingsRepository = seasonDaysMappingsRepository;
            _seasonDayTimeMappingsRepository = seasonDayTimeMappingsRepository;
            _daysRepository = daysRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _eventDetailRepository = eventDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
        }

        public SearchVenueQueryResult Handle(SearchVenueQuery query)
        {
            List<FIL.Contracts.DataModels.SearchVenue> VenueResult = new List<Contracts.DataModels.SearchVenue>();
            try
            {
                var citiesArray = query.CityName != null ? query.CityName.Split(",") : null;
                var eventCategoriesRepositoryData = _eventCategoryRepository.GetAll().Where(s => s.IsFeel == true);
                var categories = query.Categories != null ? query.Categories.Split(",") : null;
                List<long> categoryList = new List<long>();
                if (categories != null && categories.Count() > 0 && query.PlaceId == 0)
                {
                    foreach (var item in categories)
                    {
                        try
                        {
                            var categoryInfo = eventCategoriesRepositoryData.FirstOrDefault(x => x.DisplayName == item).Id;
                            categoryList.Add(categoryInfo);
                        }
                        catch (Exception ex)
                        {
                            _logger.Log(Logging.Enums.LogCategory.Error, ex);
                        }
                    }
                }
                else if (categories != null && categories.Count() == 0 && query.PlaceId == 0)
                {
                    var categoryInfo = eventCategoriesRepositoryData.FirstOrDefault(x => x.DisplayName == query.Categories).Id;
                    categoryList.Add(categoryInfo);
                }
                var cityLists = query.CityIds != null ? _cityRepository.GetByCityIds(query.CityIds) : null;
                var itineraryPlaces = query.PlaceId == 0 ? _eventRepository.GetAllItineraryPlaces(cityLists.Select(s => s.Id).ToList(), categoryList).GroupBy(x => x.Id, (key, group) => group.First()) :
                    _eventRepository.GetItinerarySearchPlaces(query.PlaceId).GroupBy(x => x.Id, (key, group) => group.First());
                var placeWeekOpenDayList = _placeWeekOpenDaysRepository.GetByEventIds(itineraryPlaces.Select(s => (long)s.Id).Distinct().ToList());
                var dayTimeMappingLists = _dayTimeMappingsRepository.GetAllByPlaceWeekOpenDays(placeWeekOpenDayList.Select(s => (long)s.Id).Distinct().ToList());
                var placeHolidayDatesRepositoryLists = _placeHolidayDatesRepository.GetAllByEventIds(itineraryPlaces.Select(s => (long)s.Id).Distinct().ToList());
                var placeVisitDurationRepositoryLists = _PlaceVisitDurationRepository.GetByEventIds(itineraryPlaces.Select(s => (long)s.Id).Distinct().ToList());
                var eventDetails = _eventDetailRepository.GetByEventIds(itineraryPlaces.Select(s => (long)s.Id).Distinct().ToList()).Where(s => s.IsEnabled == true);
                var eventTicketDetails = _eventTicketDetailRepository.GetByEventDetailIds(eventDetails.Select(s => (long)s.Id).Distinct().ToList()).Where(s => s.IsEnabled == true);
                var eventTicketAttributes = _eventTicketAttributeRepository.GetByEventTicketDetailIds(eventTicketDetails.Select(s => (long)s.Id).Distinct().ToList()).Where(s => s.IsEnabled == true);
                var ticketCategories = _ticketCategoryRepository.GetByTicketCategoryIds(eventTicketDetails.Select(s => (long)s.TicketCategoryId).Distinct().ToList());
                var Days = _daysRepository.GetAll();
                foreach (var currentPlace in itineraryPlaces)
                {
                    var PlaceVisitDuration = placeVisitDurationRepositoryLists.FirstOrDefault(x => x.EventId == currentPlace.Id);
                    var placeWeekOpenDays = placeWeekOpenDayList.Where(x => x.EventId == currentPlace.Id);
                    var dayTimeMappings = dayTimeMappingLists.Where(s => placeWeekOpenDays.Any(p => p.Id == s.PlaceWeekOpenDayId));
                    SearchVenue searchVenue = new SearchVenue();
                    searchVenue.CategoryName = currentPlace.Category;
                    searchVenue.EventId = currentPlace.Id;
                    searchVenue.CityId = currentPlace.CityId;
                    searchVenue.EventSource = currentPlace.EventSource;
                    searchVenue.CityName = currentPlace.CityName;
                    searchVenue.CountryName = currentPlace.CountryName;
                    searchVenue.Currency = currentPlace.Currency;
                    searchVenue.EventName = currentPlace.Name;
                    searchVenue.EventAltId = currentPlace.AltId;
                    searchVenue.EventSlug = currentPlace.Slug;
                    searchVenue.Address = (currentPlace.AddressLineOne == null ? string.Empty : currentPlace.AddressLineOne) + "," + (currentPlace.AddressLineTwo == null ? string.Empty : currentPlace.AddressLineTwo);
                    searchVenue.Id = currentPlace.Id;
                    searchVenue.PlaceWeekOpenDays = placeWeekOpenDays.ToList();
                    searchVenue.DayTimeMappings = dayTimeMappings.ToList();
                    searchVenue.PlaceHolidaysList = placeHolidayDatesRepositoryLists.Where(x => x.EventId == currentPlace.Id).ToList();
                    searchVenue.EventDescription = currentPlace.EventDescription;
                    searchVenue.EventDetails = eventDetails.Where(x => x.EventId == currentPlace.Id).ToList();
                    searchVenue.EventTicketDetails = eventTicketDetails.Where(x => searchVenue.EventDetails.Any(s => s.Id == x.EventDetailId)).ToList();
                    searchVenue.EventTicketAttributes = eventTicketAttributes.Where(x => searchVenue.EventTicketDetails.Any(s => s.Id == x.EventTicketDetailId)).ToList();
                    searchVenue.ticketCategories = ticketCategories.Where(x => searchVenue.EventTicketDetails.Any(s => s.TicketCategoryId == x.Id)).ToList();

                    if (currentPlace.Latitude != null)
                    {
                        var latArr = currentPlace.Latitude.Split("E");
                        var longArr = currentPlace.Longitude.Split("E");
                        searchVenue.Latitude = latArr[0];
                        searchVenue.Longitude = longArr[0];
                    }
                    else
                    {
                        searchVenue.Latitude = null;
                        searchVenue.Longitude = null;
                    }
                    VenueResult.Add(searchVenue);
                }
                return new SearchVenueQueryResult() { Venues = VenueResult.Distinct().ToList(), Days = Days.ToList() };
            }
            catch (Exception e)
            {
                return new SearchVenueQueryResult() { Venues = VenueResult.Distinct().ToList() };
            }
        }
    }

    public class Quotes
    {
        public double USDAED { get; set; }
        public double USDAFN { get; set; }
        public double USDALL { get; set; }
        public double USDAMD { get; set; }
        public double USDANG { get; set; }
        public double USDAOA { get; set; }
        public double USDARS { get; set; }
        public double USDAUD { get; set; }
        public double USDAWG { get; set; }
        public double USDAZN { get; set; }
        public double USDBAM { get; set; }
        public double USDBBD { get; set; }
        public double USDBDT { get; set; }
        public double USDBGN { get; set; }
        public double USDBHD { get; set; }
        public double USDBIF { get; set; }
        public double USDBMD { get; set; }
        public double USDBND { get; set; }
        public double USDBOB { get; set; }
        public double USDBRL { get; set; }
        public double USDBSD { get; set; }
        public double USDBTC { get; set; }
        public double USDBTN { get; set; }
        public double USDBWP { get; set; }
        public double USDBYN { get; set; }
        public double USDBYR { get; set; }
        public double USDBZD { get; set; }
        public double USDCAD { get; set; }
        public double USDCDF { get; set; }
        public double USDCHF { get; set; }
        public double USDCLF { get; set; }
        public double USDCLP { get; set; }
        public double USDCNY { get; set; }
        public double USDCOP { get; set; }
        public double USDCRC { get; set; }
        public double USDCUC { get; set; }
        public double USDCUP { get; set; }
        public double USDCVE { get; set; }
        public double USDCZK { get; set; }
        public double USDDJF { get; set; }
        public double USDDKK { get; set; }
        public double USDDOP { get; set; }
        public double USDDZD { get; set; }
        public double USDEGP { get; set; }
        public double USDERN { get; set; }
        public double USDETB { get; set; }
        public double USDEUR { get; set; }
        public double USDFJD { get; set; }
        public double USDFKP { get; set; }
        public double USDGBP { get; set; }
        public double USDGEL { get; set; }
        public double USDGGP { get; set; }
        public double USDGHS { get; set; }
        public double USDGIP { get; set; }
        public double USDGMD { get; set; }
        public double USDGNF { get; set; }
        public double USDGTQ { get; set; }
        public double USDGYD { get; set; }
        public double USDHKD { get; set; }
        public double USDHNL { get; set; }
        public double USDHRK { get; set; }
        public double USDHTG { get; set; }
        public double USDHUF { get; set; }
        public double USDIDR { get; set; }
        public double USDILS { get; set; }
        public double USDIMP { get; set; }
        public double USDINR { get; set; }
        public double USDIQD { get; set; }
        public double USDIRR { get; set; }
        public double USDISK { get; set; }
        public double USDJEP { get; set; }
        public double USDJMD { get; set; }
        public double USDJOD { get; set; }
        public double USDJPY { get; set; }
        public double USDKES { get; set; }
        public double USDKGS { get; set; }
        public double USDKHR { get; set; }
        public double USDKMF { get; set; }
        public double USDKPW { get; set; }
        public double USDKRW { get; set; }
        public double USDKWD { get; set; }
        public double USDKYD { get; set; }
        public double USDKZT { get; set; }
        public double USDLAK { get; set; }
        public double USDLBP { get; set; }
        public double USDLKR { get; set; }
        public double USDLRD { get; set; }
        public double USDLSL { get; set; }
        public double USDLTL { get; set; }
        public double USDLVL { get; set; }
        public double USDLYD { get; set; }
        public double USDMAD { get; set; }
        public double USDMDL { get; set; }
        public double USDMGA { get; set; }
        public double USDMKD { get; set; }
        public double USDMMK { get; set; }
        public double USDMNT { get; set; }
        public double USDMOP { get; set; }
        public double USDMRO { get; set; }
        public double USDMUR { get; set; }
        public double USDMVR { get; set; }
        public double USDMWK { get; set; }
        public double USDMXN { get; set; }
        public double USDMYR { get; set; }
        public double USDMZN { get; set; }
        public double USDNAD { get; set; }
        public double USDNGN { get; set; }
        public double USDNIO { get; set; }
        public double USDNOK { get; set; }
        public double USDNPR { get; set; }
        public double USDNZD { get; set; }
        public double USDOMR { get; set; }
        public double USDPAB { get; set; }
        public double USDPEN { get; set; }
        public double USDPGK { get; set; }
        public double USDPHP { get; set; }
        public double USDPKR { get; set; }
        public double USDPLN { get; set; }
        public double USDPYG { get; set; }
        public double USDQAR { get; set; }
        public double USDRON { get; set; }
        public double USDRSD { get; set; }
        public double USDRUB { get; set; }
        public double USDRWF { get; set; }
        public double USDSAR { get; set; }
        public double USDSBD { get; set; }
        public double USDSCR { get; set; }
        public double USDSDG { get; set; }
        public double USDSEK { get; set; }
        public double USDSGD { get; set; }
        public double USDSHP { get; set; }
        public double USDSLL { get; set; }
        public double USDSOS { get; set; }
        public double USDSRD { get; set; }
        public double USDSTD { get; set; }
        public double USDSVC { get; set; }
        public double USDSYP { get; set; }
        public double USDSZL { get; set; }
        public double USDTHB { get; set; }
        public double USDTJS { get; set; }
        public double USDTMT { get; set; }
        public double USDTND { get; set; }
        public double USDTOP { get; set; }
        public double USDTRY { get; set; }
        public double USDTTD { get; set; }
        public double USDTWD { get; set; }
        public double USDTZS { get; set; }
        public double USDUAH { get; set; }
        public double USDUGX { get; set; }
        public double USDUSD { get; set; }
        public double USDUYU { get; set; }
        public double USDUZS { get; set; }
        public double USDVEF { get; set; }
        public double USDVND { get; set; }
        public double USDVUV { get; set; }
        public double USDWST { get; set; }
        public double USDXAF { get; set; }
        public double USDXAG { get; set; }
        public double USDXAU { get; set; }
        public double USDXCD { get; set; }
        public double USDXDR { get; set; }
        public double USDXOF { get; set; }
        public double USDXPF { get; set; }
        public double USDYER { get; set; }
        public double USDZAR { get; set; }
        public double USDZMK { get; set; }
        public double USDZMW { get; set; }
        public double USDZWL { get; set; }
    }

    public class RootObject
    {
        public bool success { get; set; }
        public string terms { get; set; }
        public string privacy { get; set; }
        public int timestamp { get; set; }
        public string source { get; set; }
        public Quotes quotes { get; set; }
    }
}