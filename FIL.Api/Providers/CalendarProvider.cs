using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.QueryResults.PlaceInventory;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Providers
{
    public interface ICalendarProvider
    {
        GetPlaceInventoryQueryResult GetCalendarData(long eventId);
    }

    public class CalendarProvider : ICalendarProvider
    {
        private readonly IEventTicketDetailRepository _eventTicketDetail;
        private readonly IEventTicketAttributeRepository _eventTicketAttribute;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IRefundPolicyRepository _refundPolicyRepository;
        private readonly IPlaceTicketRedemptionDetailRepository _placeTicketRedemptionDetailRepository;
        private readonly IPlaceDocumentTypeMappingRepository _placeDocumentTypeMappingRepository;
        private readonly IEventDeliveryTypeDetailRepository _eventDeliveryTypeDetailRepository;
        private readonly IPlaceCustomerDocumentTypeMappingRepository _placeCustomerDocumentTypeMappingRepository;
        private readonly ICustomerDocumentTypeRepository _customerDocumentTypeRepository;
        private readonly IPlaceHolidayDatesRepository _placeHolidydates;
        private readonly IPlaceWeekOffRepository _placeWeekOffRepository;
        private readonly IEventTicketDetailTicketCategoryTypeMappingRepository _eventTicketDetailTicketCategoryTypeMappingRepository;
        private readonly ICustomerInformationRepository _customerInformationRepository;
        private readonly IEventCustomerInformationMappingRepository _eventCustomerInformationMappingRepository;
        private readonly IDaysRepository _daysRepository;
        private readonly IPlaceWeekOpenDaysRepository _placeWeekOpenDaysRepository;
        private readonly IDayTimeMappingsRepository _dayTimeMappingsRepository;
        private readonly IPlaceSeasonDetailsRepository _placeSeasonDetailsRepository;
        private readonly ISeasonDayTimeMappingsRepository _seasonDayTimeMappingsRepository;
        private readonly ISeasonDaysMappingsRepository _seasonDaysMappingsRepository;
        private readonly IPlaceSpecialDayTimeMappingsRepository _placeSpecialDayTimeMappingsRepository;

        public CalendarProvider(
            IEventTicketDetailRepository eventTicketDetail,
             IPlaceHolidayDatesRepository placeHolidayDatesRepository,
            IEventTicketAttributeRepository eventTicketAttribute,
            ITicketCategoryRepository ticketCategoryRepository,
            IPlaceTicketRedemptionDetailRepository placeTicketRedemptionDetailRepository,
            IEventDeliveryTypeDetailRepository eventDeliveryTypeDetailRepository,
            IEventDetailRepository eventDetailRepository,
            IEventRepository eventRepository,
            IRefundPolicyRepository refundPolicyRepository,
            IPlaceCustomerDocumentTypeMappingRepository placeCustomerDocumentTypeMappingRepository,
            IPlaceDocumentTypeMappingRepository placeDocumentTypeMappingRepository,
            ICustomerDocumentTypeRepository customerDocumentTypeRepository,
            ICustomerInformationRepository customerInformationRepository,
            IEventCustomerInformationMappingRepository eventCustomerInformationMappingRepository,
            IEventTicketDetailTicketCategoryTypeMappingRepository eventTicketDetailTicketCategoryTypeMappingRepository,
            IDaysRepository daysRepository,
            IPlaceWeekOpenDaysRepository placeWeekOpenDaysRepository,
            IDayTimeMappingsRepository dayTimeMappingsRepository,
            IPlaceSeasonDetailsRepository placeSeasonDetailsRepository,
            ISeasonDayTimeMappingsRepository seasonDayTimeMappingsRepository,
            ISeasonDaysMappingsRepository seasonDaysMappingsRepository,
            IPlaceSpecialDayTimeMappingsRepository placeSpecialDayTimeMappingsRepository,
            IPlaceWeekOffRepository placeWeekOffRepository)
        {
            _eventTicketDetail = eventTicketDetail;
            _eventDeliveryTypeDetailRepository = eventDeliveryTypeDetailRepository;
            _eventDetailRepository = eventDetailRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _placeDocumentTypeMappingRepository = placeDocumentTypeMappingRepository;
            _eventRepository = eventRepository;
            _eventTicketAttribute = eventTicketAttribute;
            _refundPolicyRepository = refundPolicyRepository;
            _placeTicketRedemptionDetailRepository = placeTicketRedemptionDetailRepository;
            _placeCustomerDocumentTypeMappingRepository = placeCustomerDocumentTypeMappingRepository;
            _placeHolidydates = placeHolidayDatesRepository;
            _placeWeekOffRepository = placeWeekOffRepository;
            _customerInformationRepository = customerInformationRepository;
            _eventTicketDetailTicketCategoryTypeMappingRepository = eventTicketDetailTicketCategoryTypeMappingRepository;
            _customerDocumentTypeRepository = customerDocumentTypeRepository;
            _eventCustomerInformationMappingRepository = eventCustomerInformationMappingRepository;
            _daysRepository = daysRepository;
            _placeWeekOpenDaysRepository = placeWeekOpenDaysRepository;
            _dayTimeMappingsRepository = dayTimeMappingsRepository;
            _placeSeasonDetailsRepository = placeSeasonDetailsRepository;
            _seasonDayTimeMappingsRepository = seasonDayTimeMappingsRepository;
            _seasonDaysMappingsRepository = seasonDaysMappingsRepository;
            _placeSpecialDayTimeMappingsRepository = placeSpecialDayTimeMappingsRepository;
        }

        public GetPlaceInventoryQueryResult GetCalendarData(long eventId)
        {
            /*-------------------------- Regular Time --------------------------*/
            var placeWeekOpenDays = _placeWeekOpenDaysRepository.GetByEventId(eventId).ToList();
            var allDays = _daysRepository.GetAll().ToList();
            var regular = new RegularViewModel();
            if (placeWeekOpenDays.Any())
            {
                bool[] openDays = new bool[8];
                var i = 0;
                foreach (FIL.Contracts.DataModels.PlaceWeekOpenDays currentPlaceWeek in placeWeekOpenDays)
                {
                    openDays[currentPlaceWeek.DayId] = true;
                    i = i + 1;
                }
                if (i == 7)
                {
                    openDays[0] = true;
                }
                if (placeWeekOpenDays[0].IsSameTime)
                {
                    i = 0;
                    var dayTimeMappings = _dayTimeMappingsRepository.GetAllByPlaceWeekOpenDay(placeWeekOpenDays[0].Id).ToList();
                    List<TimeViewModel> timeModels = new List<TimeViewModel>();
                    List<CustomTimeModelData> customTimeModels = new List<CustomTimeModelData>();
                    foreach (FIL.Contracts.DataModels.DayTimeMappings daysTime in dayTimeMappings)
                    {
                        var newTimeModel = new TimeViewModel();
                        newTimeModel.Id = i + 1;
                        newTimeModel.From = daysTime.StartTime;
                        newTimeModel.To = daysTime.EndTime;
                        i = i + 1;
                        timeModels.Add(newTimeModel);
                    }
                    regular.TimeModel = timeModels;
                    i = 0;
                    foreach (FIL.Contracts.DataModels.PlaceWeekOpenDays currentPlaceWeek in placeWeekOpenDays)
                    {
                        var newCustomTimeModel = new CustomTimeModelData();
                        timeModels = new List<TimeViewModel>();
                        var newTimeModel = new TimeViewModel();
                        var currentDay = _daysRepository.Get(currentPlaceWeek.DayId);
                        newCustomTimeModel.Id = i + 1;
                        newCustomTimeModel.Day = currentDay.Name;
                        newTimeModel.Id = i + 1;
                        newTimeModel.From = "";
                        newTimeModel.To = "";
                        timeModels.Add(newTimeModel);
                        newCustomTimeModel.Time = timeModels;
                        customTimeModels.Add(newCustomTimeModel);
                        i = i + 1;
                    }
                    regular.CustomTimeModel = customTimeModels;
                }
                else if (!placeWeekOpenDays[0].IsSameTime)
                {
                    i = 0;
                    var dayTimeMappings = _dayTimeMappingsRepository.GetAllByPlaceWeekOpenDay(placeWeekOpenDays[0].Id).ToList();
                    List<TimeViewModel> timeModels = new List<TimeViewModel>();
                    List<CustomTimeModelData> customTimeModels = new List<CustomTimeModelData>();
                    var newTimeModel = new TimeViewModel();
                    newTimeModel.Id = i + 1;
                    newTimeModel.From = "";
                    newTimeModel.To = "";
                    timeModels.Add(newTimeModel);
                    regular.TimeModel = timeModels;
                    i = 0;
                    foreach (FIL.Contracts.DataModels.PlaceWeekOpenDays currentPlaceWeek in placeWeekOpenDays)
                    {
                        var newCustomTimeModel = new CustomTimeModelData();
                        var currentDayTimeMappings = _dayTimeMappingsRepository.GetAllByPlaceWeekOpenDay(currentPlaceWeek.Id).ToList();
                        var currentDay = _daysRepository.Get(currentPlaceWeek.DayId);
                        timeModels = new List<TimeViewModel>();
                        var index = 0;
                        foreach (FIL.Contracts.DataModels.DayTimeMappings currentTime in currentDayTimeMappings)
                        {
                            var currentTimeModel = new TimeViewModel();
                            currentTimeModel.Id = index + 1;
                            currentTimeModel.From = currentTime.StartTime;
                            currentTimeModel.To = currentTime.EndTime;
                            timeModels.Add(currentTimeModel);
                            index = index + 1;
                        }
                        newCustomTimeModel.Time = timeModels;
                        newCustomTimeModel.Day = currentDay.Name;
                        newCustomTimeModel.Id = i + 1;
                        customTimeModels.Add(newCustomTimeModel);
                        i = i + 1;
                    }
                    regular.CustomTimeModel = customTimeModels;
                }
                regular.IsSameTime = placeWeekOpenDays[0].IsSameTime;
                regular.DaysOpen = openDays.OfType<bool>().ToList();
            }
            else
            {
                if (regular.CustomTimeModel == null)
                {
                    regular.CustomTimeModel = new List<CustomTimeModelData>();
                }
                if (regular.TimeModel == null)
                {
                    regular.TimeModel = new List<TimeViewModel>();
                }
                if (regular.DaysOpen == null)
                {
                    bool[] openDays = new bool[8];
                    regular.DaysOpen = openDays.OfType<bool>().ToList();
                }
                regular.IsSameTime = true;
            }

            /*-------------------------- Season Time --------------------------*/
            var season = new List<SeasonViewModel>();
            var placeSeasonDetails = _placeSeasonDetailsRepository.GetByEventId(eventId).ToList();
            var seasonIndex = 0;
            foreach (FIL.Contracts.DataModels.PlaceSeasonDetails currentSeason in placeSeasonDetails)
            {
                var seasonDaysMappings = _seasonDaysMappingsRepository.GetByPlaceSeasonDetailId(currentSeason.Id).ToList();
                var newSeasonViewModel = new SeasonViewModel();
                List<TimeViewModel> timeModelList = new List<TimeViewModel>();
                List<SpeecialDateSeasonTimeViewModel> speecialDateSeasonTimeList = new List<SpeecialDateSeasonTimeViewModel>();
                bool[] openDays = new bool[8];

                if (currentSeason.IsSameTime && seasonDaysMappings.Any())
                {
                    var seasonSameTiming = _seasonDayTimeMappingsRepository.GetSeasonDaysMappings(seasonDaysMappings[0].Id).ToList();

                    var timeIndex = 0;
                    foreach (FIL.Contracts.DataModels.SeasonDaysTimeMapping currentSeasonTime in seasonSameTiming)
                    {
                        var timeModel = new TimeViewModel();
                        timeModel.Id = timeIndex;
                        timeModel.From = currentSeasonTime.StartTime;
                        timeModel.To = currentSeasonTime.EndTime;
                        timeIndex = timeIndex + 1;
                        timeModelList.Add(timeModel);
                    }
                    timeIndex = 0;
                    foreach (FIL.Contracts.DataModels.SeasonDaysMapping currentSeasonDay in seasonDaysMappings)
                    {
                        var speecialDateSeasonTime = new SpeecialDateSeasonTimeViewModel();

                        var currentDay = _daysRepository.Get(currentSeasonDay.DayId);
                        var timeModels = new List<TimeViewModel>();
                        var timeModel = new TimeViewModel();
                        timeModel.Id = 1;
                        timeModel.From = "";
                        timeModel.To = "";
                        timeModels.Add(timeModel);
                        speecialDateSeasonTime.Id = timeIndex + 1;
                        speecialDateSeasonTime.Day = currentDay.Name;
                        speecialDateSeasonTime.Time = timeModels;
                        openDays[currentSeasonDay.DayId] = true;
                        timeIndex = timeIndex + 1;
                        speecialDateSeasonTimeList.Add(speecialDateSeasonTime);
                    }
                }
                else if (!currentSeason.IsSameTime)
                {
                    var timeIndex = 0;
                    var timeModel = new TimeViewModel();
                    timeModel.Id = 1;
                    timeModel.From = "";
                    timeModel.To = "";
                    timeModelList.Add(timeModel);
                    foreach (FIL.Contracts.DataModels.SeasonDaysMapping currentSeasonDay in seasonDaysMappings)
                    {
                        var speecialDateSeasonTime = new SpeecialDateSeasonTimeViewModel();
                        var currentDay = _daysRepository.Get(currentSeasonDay.DayId);
                        var timeModels = new List<TimeViewModel>();
                        var seasonSpecialTime = _seasonDayTimeMappingsRepository.GetSeasonDaysMappings(currentSeasonDay.Id);
                        var seasonTimeIndex = 0;
                        foreach (FIL.Contracts.DataModels.SeasonDaysTimeMapping currentSeasonDayTime in seasonSpecialTime)
                        {
                            timeModel = new TimeViewModel();
                            timeModel.Id = seasonTimeIndex + 1;
                            timeModel.To = currentSeasonDayTime.EndTime;
                            timeModel.From = currentSeasonDayTime.StartTime;
                            timeModels.Add(timeModel);
                            seasonTimeIndex = seasonTimeIndex + 1;
                        }
                        openDays[currentSeasonDay.DayId] = true;
                        speecialDateSeasonTime.Id = timeIndex + 1;
                        speecialDateSeasonTime.Day = currentDay.Name;
                        speecialDateSeasonTime.Time = timeModels;
                        speecialDateSeasonTimeList.Add(speecialDateSeasonTime);
                        timeIndex = timeIndex + 1;
                    }
                }
                newSeasonViewModel.Id = seasonIndex + 1;
                newSeasonViewModel.SameTime = timeModelList;
                newSeasonViewModel.Time = speecialDateSeasonTimeList;
                newSeasonViewModel.StartDate = currentSeason.StartDate.AddHours(5).AddMinutes(30);
                newSeasonViewModel.EndDate = currentSeason.EndDate.AddHours(5).AddMinutes(30);
                newSeasonViewModel.Name = currentSeason.Name;
                newSeasonViewModel.IsSameTime = currentSeason.IsSameTime;
                newSeasonViewModel.DaysOpen = openDays.OfType<bool>().ToList();
                seasonIndex = seasonIndex + 1;
                season.Add(newSeasonViewModel);
            }
            /*-------------------------- Special day/Holiday Time --------------------------*/
            var specialDay = new List<SpecialDayViewModel>();
            var specialDays = _placeSpecialDayTimeMappingsRepository.GetAllByEventId(eventId);
            var specialDayindex = 0;
            foreach (FIL.Contracts.DataModels.PlaceSpecialDayTimeMappings currentSpecialDay in specialDays)
            {
                var newSpecialDay = new SpecialDayViewModel();
                newSpecialDay.Id = specialDayindex + 1;
                newSpecialDay.Name = currentSpecialDay.Name;
                newSpecialDay.From = currentSpecialDay.StartTime;
                newSpecialDay.To = currentSpecialDay.EndTime;
                newSpecialDay.SpecialDate = currentSpecialDay.SpecialDate.AddHours(5).AddMinutes(30);
                specialDayindex = specialDayindex + 1;
                specialDay.Add(newSpecialDay);
            }

            return new GetPlaceInventoryQueryResult
            {
                SeasonTimeModel = season,
                SpecialDayModel = specialDay,
                RegularTimeModel = regular
            };
        }
    }
}