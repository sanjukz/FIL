using FIL.Api.CommandHandlers;
using FIL.Api.Repositories;
using FIL.Contracts.Commands.PlaceCalendar;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.QueryHandlers.PlaceCalendar
{
    public class PlaceCalendarCommandHandler : BaseCommandHandlerWithResult<PlaceCalendarCommand, PlaceCalendarCommandResult>
    {
        private readonly IMediator _mediator;
        private readonly IPlaceHolidayDatesRepository _placeHolidydates;
        private readonly IPlaceWeekOffRepository _placeWeekOffRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetail;
        private readonly IEventTicketAttributeRepository _eventTicketAttribute;
        private readonly IDaysRepository _daysRepository;
        private readonly IPlaceWeekOpenDaysRepository _placeWeekOpenDaysRepository;
        private readonly IDayTimeMappingsRepository _dayTimeMappingsRepository;
        private readonly IPlaceSeasonDetailsRepository _placeSeasonDetailsRepository;
        private readonly ISeasonDayTimeMappingsRepository _seasonDayTimeMappingsRepository;
        private readonly ISeasonDaysMappingsRepository _seasonDaysMappingsRepository;
        private readonly IPlaceSpecialDayTimeMappingsRepository _placeSpecialDayTimeMappingsRepository;

        public PlaceCalendarCommandHandler(IMediator mediator,
            IPlaceHolidayDatesRepository placeHolidayDatesRepository,
             IEventTicketDetailRepository eventTicketDetail,
            IEventTicketAttributeRepository eventTicketAttribute,
            IEventDetailRepository eventDetailRepository,
            IEventRepository eventRepository,
            IVenueRepository venueRepository,
            IDaysRepository daysRepository,
            IPlaceWeekOpenDaysRepository placeWeekOpenDaysRepository,
            IDayTimeMappingsRepository dayTimeMappingsRepository,
            IPlaceSeasonDetailsRepository placeSeasonDetailsRepository,
            ISeasonDayTimeMappingsRepository seasonDayTimeMappingsRepository,
            ISeasonDaysMappingsRepository seasonDaysMappingsRepository,
            IPlaceSpecialDayTimeMappingsRepository placeSpecialDayTimeMappingsRepository,
            IPlaceWeekOffRepository placeWeekOffRepository) : base(mediator)
        {
            _mediator = mediator;
            _placeHolidydates = placeHolidayDatesRepository;
            _placeWeekOffRepository = placeWeekOffRepository;
            _eventDetailRepository = eventDetailRepository;
            _eventRepository = eventRepository;
            _venueRepository = venueRepository;
            _eventTicketDetail = eventTicketDetail;
            _eventTicketAttribute = eventTicketAttribute;
            _daysRepository = daysRepository;
            _placeWeekOpenDaysRepository = placeWeekOpenDaysRepository;
            _dayTimeMappingsRepository = dayTimeMappingsRepository;
            _placeSeasonDetailsRepository = placeSeasonDetailsRepository;
            _seasonDayTimeMappingsRepository = seasonDayTimeMappingsRepository;
            _seasonDaysMappingsRepository = seasonDaysMappingsRepository;
            _placeSpecialDayTimeMappingsRepository = placeSpecialDayTimeMappingsRepository;
        }

        public void CreateNewHolidyCalendar(List<DateTime> dateTimes, long eventDetailId, long eventId)
        {
            foreach (DateTime date in dateTimes)
            {
                PlaceHolidayDate placeHolidayDate = new PlaceHolidayDate();
                placeHolidayDate.EventId = eventId;
                placeHolidayDate.LeaveDateTime = date;
                placeHolidayDate.CreatedUtc = DateTime.UtcNow;
                placeHolidayDate.IsEnabled = true;
                _placeHolidydates.Save(placeHolidayDate);
            };
        }

        public void CreatePlaceWeekOffs(int weeklyOffDaysCount, long eventDetailId, List<Boolean> WeekOffDays, long eventId)
        {
            if (weeklyOffDaysCount == 7) // All days off...
            {
                PlaceWeekOff placeWeekOff = new PlaceWeekOff();
                placeWeekOff.EventId = eventId;
                placeWeekOff.WeekOffDay = FIL.Contracts.Enums.WeekOffDays.All;
                placeWeekOff.CreatedUtc = DateTime.UtcNow;
                placeWeekOff.IsEnabled = true;
                placeWeekOff.CreatedBy = Guid.NewGuid();
                _placeWeekOffRepository.Save(placeWeekOff);
            }
            else
            {
                try
                {
                    var offDayIndex = 0;
                    foreach (bool isOff in WeekOffDays)
                    {
                        offDayIndex = offDayIndex + 1;
                        if (isOff)
                        {
                            PlaceWeekOff placeWeekOff = new PlaceWeekOff();
                            placeWeekOff.EventId = eventId;
                            placeWeekOff.WeekOffDay = (WeekOffDays)Enum.ToObject(typeof(WeekOffDays), offDayIndex);
                            placeWeekOff.CreatedUtc = DateTime.UtcNow;
                            placeWeekOff.IsEnabled = true;
                            placeWeekOff.CreatedBy = Guid.NewGuid();
                            _placeWeekOffRepository.Save(placeWeekOff);
                        }
                    };
                }
                catch (Exception e)
                {
                }
            }
        }

        public void UpdatePlaceWeekOff(int weeklyOffDaysCount, long eventDetailId, List<Boolean> WeekOffDays, long eventId)
        {
            try
            {
                var placeWeekOff = _placeWeekOffRepository.GetAllByEventId(eventId);
                foreach (FIL.Contracts.DataModels.PlaceWeekOff placeWeek in placeWeekOff)
                {
                    _placeWeekOffRepository.Delete(placeWeek);
                }
                CreatePlaceWeekOffs(weeklyOffDaysCount, eventDetailId, WeekOffDays, eventId);
            }
            catch (Exception e)
            {
            }
        }

        public void UpdatePlaceHoliday(List<DateTime> dateTimes, long eventDetailId, long eventId)
        {
            try
            {
                var placeHolidayDates = _placeHolidydates.GetAllByEventId(eventId);
                foreach (FIL.Contracts.DataModels.PlaceHolidayDate placeHolidayDate in placeHolidayDates)
                {
                    _placeHolidydates.Delete(placeHolidayDate);
                }
                CreateNewHolidyCalendar(dateTimes, eventDetailId, eventId);
            }
            catch (Exception e)
            {
            }
        }

        public EventTicketAttribute CreateEventTicketAttributes
         (long currentEventTicketDetail,
         string ticketCategoryNote,
         int CurrencyId,
         int Quantity,
         string TicketCategoryDescription,
         float PricePerTicket,
         bool IsRollingTicketValidityType,
         string Days,
         string Month,
         string Year,
         DateTime TicketValidityFixDate
         )
        {
            EventTicketAttribute eventTicketAttribute = new EventTicketAttribute();
            eventTicketAttribute.EventTicketDetailId = currentEventTicketDetail;
            eventTicketAttribute.TicketCategoryNotes = ticketCategoryNote;
            eventTicketAttribute.SalesStartDateTime = DateTime.UtcNow;
            eventTicketAttribute.SalesEndDatetime = DateTime.UtcNow;
            eventTicketAttribute.TicketTypeId = TicketType.Regular;
            eventTicketAttribute.ChannelId = Channels.Feel;
            eventTicketAttribute.CurrencyId = CurrencyId;
            eventTicketAttribute.AvailableTicketForSale = Quantity;
            eventTicketAttribute.RemainingTicketForSale = Quantity;
            eventTicketAttribute.TicketCategoryDescription = (TicketCategoryDescription == null ? "" : TicketCategoryDescription);
            eventTicketAttribute.ViewFromStand = "";
            eventTicketAttribute.IsSeatSelection = false;
            eventTicketAttribute.Price = (decimal)PricePerTicket;
            eventTicketAttribute.IsInternationalCardAllowed = true;
            eventTicketAttribute.IsEMIApplicable = false;
            eventTicketAttribute.IsEnabled = true;
            eventTicketAttribute.TicketValidityType = IsRollingTicketValidityType ? TicketValidityTypes.Rolling : TicketValidityTypes.Fixed;
            eventTicketAttribute.CreatedUtc = DateTime.UtcNow;
            eventTicketAttribute.CreatedBy = Guid.NewGuid();
            if (IsRollingTicketValidityType)
            {
                eventTicketAttribute.TicketValidity = Days + "-" + Month + "-" + Year;
            }
            else
            {
                eventTicketAttribute.TicketValidity = TicketValidityFixDate.ToString();
            }
            var currentEventTicketAttribute = _eventTicketAttribute.Save(eventTicketAttribute);
            return currentEventTicketAttribute;
        }

        public FIL.Contracts.DataModels.EventTicketDetail CreateEventTicketDetails(long eventDetailId, int ticketCatId)
        {
            EventTicketDetail eventTicketDetail = new EventTicketDetail();
            eventTicketDetail.EventDetailId = eventDetailId;
            eventTicketDetail.TicketCategoryId = ticketCatId;
            eventTicketDetail.IsEnabled = true;
            eventTicketDetail.CreatedUtc = DateTime.UtcNow;
            eventTicketDetail.CreatedBy = Guid.NewGuid();
            var data = _eventTicketDetail.Save(eventTicketDetail);
            return data;
        }

        public EventDetail CreateEventDetail(string placeName, Guid guid, long placeId, int venueId, DateTime From, DateTime To, bool isCreateEventTicketDetail)
        {
            var existedEventDetail = _eventDetailRepository.GetSubeventByEventId((int)placeId).FirstOrDefault();

            EventDetail placeDetails = new EventDetail();
            placeDetails.Name = placeName;
            placeDetails.AltId = guid;
            placeDetails.EventId = placeId;
            placeDetails.VenueId = venueId;
            placeDetails.IsEnabled = true;
            placeDetails.GroupId = 1;
            placeDetails.StartDateTime = From;
            placeDetails.EndDateTime = To;
            placeDetails.CreatedUtc = DateTime.UtcNow;
            placeDetails.CreatedBy = Guid.NewGuid();
            var currentEventDetail = _eventDetailRepository.Save(placeDetails);

            try
            {
                if (isCreateEventTicketDetail && existedEventDetail != null) // if new time
                {
                    var eventTicketDetailData = _eventTicketDetail.GetByEventDetailId(existedEventDetail.Id);
                    foreach (FIL.Contracts.DataModels.EventTicketDetail current in eventTicketDetailData)
                    {
                        var eventTicketAttributes = _eventTicketAttribute.GetByEventTicketDetailsId(current.Id);

                        if (eventTicketAttributes != null)
                        {
                            var createdEventTicketDetail = CreateEventTicketDetails(currentEventDetail.Id, (int)current.TicketCategoryId);
                            CreateEventTicketAttributes(createdEventTicketDetail.Id, eventTicketAttributes.TicketCategoryNotes, eventTicketAttributes.CurrencyId,
                                eventTicketAttributes.AvailableTicketForSale, eventTicketAttributes.TicketCategoryDescription, (float)eventTicketAttributes.Price,
                                true, "", "", "", new DateTime());
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }

            return placeDetails;
        }

        public void updateEventStatus(long eventId, bool isTrue)
        {
            try
            {
                var currentEvent = _eventRepository.Get(eventId);
                currentEvent.IsEnabled = isTrue;
                currentEvent.IsCreatedFromFeelAdmin = true;
                _eventRepository.Save(currentEvent);
            }
            catch (Exception e)
            {
            }
        }

        public void deleteCalendar(Event place)
        {
            try
            {
                var placeWeekDays = _placeWeekOpenDaysRepository.GetByEventId(place.Id).ToList();
                var DayTimeMappings = _dayTimeMappingsRepository.GetAllByPlaceWeekOpenDays(placeWeekDays.Select(s => s.Id).ToList());
                var PlaceSeasonDetails = _placeSeasonDetailsRepository.GetByEventId(place.Id);
                var placeSeasonDays = _seasonDaysMappingsRepository.GetByPlaceSeasonDetailIds(PlaceSeasonDetails.Select(s => s.Id).ToList());
                var seasonDaysTimeMappings = _seasonDayTimeMappingsRepository.GetAllSeasonDaysMappings(placeSeasonDays.Select(s => s.Id).ToList());
                var specialDayTimeMappings = _placeSpecialDayTimeMappingsRepository.GetAllByEventId((place.Id));

                foreach (FIL.Contracts.DataModels.DayTimeMappings currentDay in DayTimeMappings)
                {
                    _dayTimeMappingsRepository.Delete(currentDay);
                }
                foreach (FIL.Contracts.DataModels.PlaceWeekOpenDays currentDay in placeWeekDays)
                {
                    _placeWeekOpenDaysRepository.Delete(currentDay);
                }
                foreach (FIL.Contracts.DataModels.SeasonDaysTimeMapping currentDay in seasonDaysTimeMappings)
                {
                    _seasonDayTimeMappingsRepository.Delete(currentDay);
                }
                foreach (FIL.Contracts.DataModels.SeasonDaysTimeMapping currentDay in seasonDaysTimeMappings)
                {
                    _seasonDayTimeMappingsRepository.Delete(currentDay);
                }
                foreach (FIL.Contracts.DataModels.SeasonDaysMapping currentDay in placeSeasonDays)
                {
                    _seasonDaysMappingsRepository.Delete(currentDay);
                }
                foreach (FIL.Contracts.DataModels.PlaceSeasonDetails currentDay in PlaceSeasonDetails)
                {
                    _placeSeasonDetailsRepository.Delete(currentDay);
                }
                foreach (FIL.Contracts.DataModels.PlaceSpecialDayTimeMappings currentDay in specialDayTimeMappings)
                {
                    _placeSpecialDayTimeMappingsRepository.Delete(currentDay);
                }
            }
            catch (Exception e)
            {
            }
        }

        public void addCalendar(PlaceCalendarCommand command, Event place)
        {
            try
            {
                /*------------------------- Regular Time ----------------------------*/
                var currentIndex = 0;
                foreach (bool currentDay in command.RegularTimeModel.DaysOpen)
                {
                    if (currentIndex > 0 && currentDay)
                    {
                        var placeWeekOpenDays = new PlaceWeekOpenDays();
                        placeWeekOpenDays.AltId = Guid.NewGuid();
                        placeWeekOpenDays.DayId = currentIndex;
                        placeWeekOpenDays.EventId = place.Id;
                        placeWeekOpenDays.IsSameTime = command.RegularTimeModel.IsSameTime;
                        placeWeekOpenDays.IsEnabled = true;
                        placeWeekOpenDays.CreatedUtc = DateTime.UtcNow;
                        placeWeekOpenDays.UpdatedUtc = DateTime.UtcNow;
                        placeWeekOpenDays.CreatedBy = Guid.NewGuid();
                        placeWeekOpenDays.UpdatedBy = Guid.NewGuid();
                        _placeWeekOpenDaysRepository.Save(placeWeekOpenDays);
                    }
                    currentIndex = currentIndex + 1;
                }
                if (command.RegularTimeModel.IsSameTime)
                {
                    var placeWeekOpnday = _placeWeekOpenDaysRepository.GetByEventId(place.Id);
                    foreach (FIL.Contracts.DataModels.PlaceWeekOpenDays currentDay in placeWeekOpnday)
                    {
                        foreach (FIL.Contracts.Commands.PlaceCalendar.TimeViewModel currentTime in command.RegularTimeModel.TimeModel)
                        {
                            var dayTimeMapping = new DayTimeMappings();
                            dayTimeMapping.AltId = Guid.NewGuid();
                            dayTimeMapping.StartTime = currentTime.From;
                            dayTimeMapping.EndTime = currentTime.To;
                            dayTimeMapping.PlaceWeekOpenDayId = currentDay.Id;
                            dayTimeMapping.IsEnabled = true;
                            dayTimeMapping.CreatedUtc = DateTime.UtcNow;
                            dayTimeMapping.UpdatedUtc = DateTime.UtcNow;
                            dayTimeMapping.CreatedBy = Guid.NewGuid();
                            dayTimeMapping.UpdatedBy = Guid.NewGuid();
                            _dayTimeMappingsRepository.Save(dayTimeMapping);
                        }
                    }
                }
                if (!command.RegularTimeModel.IsSameTime)
                {
                    var placeWeekOpnday = _placeWeekOpenDaysRepository.GetByEventId(place.Id);
                    foreach (FIL.Contracts.DataModels.PlaceWeekOpenDays currentDay in placeWeekOpnday)
                    {
                        foreach (FIL.Contracts.Commands.PlaceCalendar.CustomTimeModelData currentTime in command.RegularTimeModel.CustomTimeModel)
                        {
                            var currentDays = _daysRepository.GetByDayname(currentTime.Day);
                            if (currentDays.Id == currentDay.DayId)
                            {
                                foreach (FIL.Contracts.Commands.PlaceCalendar.TimeViewModel time in currentTime.Time)
                                {
                                    var dayTimeMapping = new DayTimeMappings();
                                    dayTimeMapping.AltId = Guid.NewGuid();
                                    dayTimeMapping.StartTime = time.From;
                                    dayTimeMapping.EndTime = time.To;
                                    dayTimeMapping.PlaceWeekOpenDayId = currentDay.Id;
                                    dayTimeMapping.IsEnabled = true;
                                    dayTimeMapping.CreatedUtc = DateTime.UtcNow;
                                    dayTimeMapping.UpdatedUtc = DateTime.UtcNow;
                                    dayTimeMapping.CreatedBy = Guid.NewGuid();
                                    dayTimeMapping.UpdatedBy = Guid.NewGuid();
                                    _dayTimeMappingsRepository.Save(dayTimeMapping);
                                }
                            }
                        }
                    }
                }
                /*------------------------- Season Time ----------------------------*/
                foreach (FIL.Contracts.Commands.PlaceCalendar.SeasonViewModel currentSeason in command.SeasonTimeModel)
                {
                    var currentDayIndex = 0;
                    var placeSeasonDetail = new PlaceSeasonDetails();
                    placeSeasonDetail.Name = currentSeason.Name;
                    placeSeasonDetail.StartDate = currentSeason.StartDate;
                    placeSeasonDetail.EndDate = currentSeason.EndDate;
                    placeSeasonDetail.AltId = Guid.NewGuid();
                    placeSeasonDetail.EventId = place.Id;
                    placeSeasonDetail.IsSameTime = currentSeason.IsSameTime;
                    placeSeasonDetail.IsEnabled = true;
                    placeSeasonDetail.CreatedUtc = DateTime.UtcNow;
                    placeSeasonDetail.UpdatedUtc = DateTime.UtcNow;
                    placeSeasonDetail.CreatedBy = Guid.NewGuid();
                    placeSeasonDetail.UpdatedBy = Guid.NewGuid();
                    var currentSeasonModel = _placeSeasonDetailsRepository.Save(placeSeasonDetail);

                    foreach (bool currentDay in currentSeason.DaysOpen)
                    {
                        if (currentDayIndex > 0 && currentDay)
                        {
                            var day = _daysRepository.Get(currentDayIndex);
                            var seasonDayMappingModel = new SeasonDaysMapping();
                            seasonDayMappingModel.AltId = Guid.NewGuid();
                            seasonDayMappingModel.DayId = currentDayIndex;
                            seasonDayMappingModel.PlaceSeasonDetailId = currentSeasonModel.Id;
                            seasonDayMappingModel.IsEnabled = true;
                            seasonDayMappingModel.CreatedUtc = DateTime.UtcNow;
                            seasonDayMappingModel.UpdatedUtc = DateTime.UtcNow;
                            seasonDayMappingModel.CreatedBy = Guid.NewGuid();
                            seasonDayMappingModel.UpdatedBy = Guid.NewGuid();
                            var seasonDay = _seasonDaysMappingsRepository.Save(seasonDayMappingModel);

                            if (currentSeason.IsSameTime)
                            {
                                foreach (FIL.Contracts.Commands.PlaceCalendar.TimeViewModel currentTime in currentSeason.SameTime)
                                {
                                    var seasonDayTimeMappings = new SeasonDaysTimeMapping();
                                    seasonDayTimeMappings.SeasonDaysMappingId = seasonDay.Id;
                                    seasonDayTimeMappings.AltId = Guid.NewGuid();
                                    seasonDayTimeMappings.StartTime = currentTime.From;
                                    seasonDayTimeMappings.EndTime = currentTime.To;
                                    seasonDayTimeMappings.IsEnabled = true;
                                    seasonDayTimeMappings.CreatedUtc = DateTime.UtcNow;
                                    seasonDayTimeMappings.UpdatedUtc = DateTime.UtcNow;
                                    seasonDayTimeMappings.CreatedBy = Guid.NewGuid();
                                    seasonDayTimeMappings.UpdatedBy = Guid.NewGuid();
                                    _seasonDayTimeMappingsRepository.Save(seasonDayTimeMappings);
                                }
                            }
                            else if (!currentSeason.IsSameTime)
                            {
                                foreach (FIL.Contracts.Commands.PlaceCalendar.SpeecialDateSeasonTimeViewModel currentSpeecialDateSeasonTimeViewModel in currentSeason.Time)
                                {
                                    if (currentSpeecialDateSeasonTimeViewModel.Day == day.Name)
                                    {
                                        foreach (FIL.Contracts.Commands.PlaceCalendar.TimeViewModel currentTime in currentSpeecialDateSeasonTimeViewModel.Time)
                                        {
                                            var seasonDayTimeMappings = new SeasonDaysTimeMapping();
                                            seasonDayTimeMappings.SeasonDaysMappingId = seasonDay.Id;
                                            seasonDayTimeMappings.AltId = Guid.NewGuid();
                                            seasonDayTimeMappings.StartTime = currentTime.From;
                                            seasonDayTimeMappings.EndTime = currentTime.To;
                                            seasonDayTimeMappings.IsEnabled = true;
                                            seasonDayTimeMappings.CreatedUtc = DateTime.UtcNow;
                                            seasonDayTimeMappings.UpdatedUtc = DateTime.UtcNow;
                                            seasonDayTimeMappings.CreatedBy = Guid.NewGuid();
                                            seasonDayTimeMappings.UpdatedBy = Guid.NewGuid();
                                            _seasonDayTimeMappingsRepository.Save(seasonDayTimeMappings);
                                        }
                                    }
                                }
                            }
                        }
                        currentDayIndex = currentDayIndex + 1;
                    }
                }
                /*------------------------- Special Day/Holiday Time ----------------------------*/
                foreach (FIL.Contracts.Commands.PlaceCalendar.SpecialDayViewModel currentSpecialDay in command.SpecialDayModel)
                {
                    var specialDay = new PlaceSpecialDayTimeMappings();
                    specialDay.Name = currentSpecialDay.Name;
                    specialDay.SpecialDate = currentSpecialDay.SpecialDate;
                    specialDay.StartTime = currentSpecialDay.From;
                    specialDay.EndTime = currentSpecialDay.To;
                    specialDay.EventId = place.Id;
                    specialDay.AltId = Guid.NewGuid();
                    specialDay.IsEnabled = true;
                    specialDay.CreatedUtc = DateTime.UtcNow;
                    specialDay.UpdatedUtc = DateTime.UtcNow;
                    specialDay.CreatedBy = Guid.NewGuid();
                    specialDay.UpdatedBy = Guid.NewGuid();
                    _placeSpecialDayTimeMappingsRepository.Save(specialDay);
                }
            }
            catch (Exception e)
            {
            }
        }

        protected override async Task<ICommandResult> Handle(PlaceCalendarCommand command)
        {
            try
            {
                var StartDateTime = DateTime.UtcNow;
                var EndDateTime = DateTime.UtcNow.AddYears(2);
                List<EventDetail> createdEventDetailList = new List<EventDetail>();
                if (command.PlaceType == FIL.Contracts.Enums.EventType.Regular)
                {
                    StartDateTime = command.PlaceStartDate;
                    EndDateTime = command.PlaceEndDate;
                }
                var place = _eventRepository.GetByAltId(command.PlaceAltId);

                var placeDetail = _eventDetailRepository.GetSubEventByEventId(place.Id).ToList();
                var currentPlaceDetail = _eventDetailRepository.GetByEvent(place.Id).ToList();
                if (!currentPlaceDetail.Any())
                {
                    try
                    {
                        foreach (FIL.Contracts.DataModels.EventDetail currentEventDetail in currentPlaceDetail)
                        {
                            currentEventDetail.IsEnabled = false;
                            currentEventDetail.EventId = 2384;
                            _eventDetailRepository.Save(currentEventDetail);
                        }
                    }
                    catch (Exception e)
                    {
                    }
                    EventDetail eventDetail = new EventDetail();
                    eventDetail.AltId = Guid.NewGuid();
                    eventDetail.EventId = place.Id;
                    eventDetail.Description = "";
                    eventDetail.IsEnabled = true;
                    eventDetail.Name = place.Name;
                    eventDetail.VenueId = 9893;
                    eventDetail.IsEnabled = true;
                    eventDetail.CreatedBy = command.ModifiedBy;
                    eventDetail.CreatedUtc = DateTime.Now;
                    eventDetail.UpdatedBy = command.ModifiedBy;
                    eventDetail.ModifiedBy = command.ModifiedBy;
                    eventDetail.UpdatedUtc = DateTime.Now;
                    eventDetail.StartDateTime = StartDateTime;
                    eventDetail.EndDateTime = EndDateTime;
                    eventDetail.GroupId = 1;
                    _eventDetailRepository.Save(eventDetail);
                }
                var index = -1;
                int venueId = 1;
                //DateTime From = new DateTime(StartDateTime.Year, StartDateTime.Month, StartDateTime.Day, StartDateTime.Hour, StartDateTime.Minute, 0).ToUniversalTime();
                var From = StartDateTime;
                var To = EndDateTime;
                //DateTime To = new DateTime(EndDateTime.Year, EndDateTime.Month, EndDateTime.Day, EndDateTime.Hour, EndDateTime.Minute, 0).ToUniversalTime();
                if (!command.IsEdit) // if new creation
                {
                    place.EventTypeId = command.PlaceType;
                    _eventRepository.Save(place);
                    index = index + 1;
                    Guid guid = Guid.NewGuid();
                    venueId = placeDetail.ElementAt(0).VenueId;
                    placeDetail.ElementAt(0).Name = place.Name;
                    placeDetail.ElementAt(0).AltId = guid;
                    placeDetail.ElementAt(0).StartDateTime = From;
                    placeDetail.ElementAt(0).EndDateTime = To;
                    placeDetail.ElementAt(0).UpdatedUtc = DateTime.UtcNow;
                    _eventDetailRepository.Save(placeDetail.ElementAt(0));
                    deleteCalendar(place);
                    addCalendar(command, place);
                    var eventDetail = _eventDetailRepository.GetByAltId(guid);
                    createdEventDetailList.Add(eventDetail);
                    CreateNewHolidyCalendar(command.HolidayDates, eventDetail.Id, place.Id);
                    var weeklyOffDaysCount = command.WeekOffDays.Where(s => s).Count();
                    CreatePlaceWeekOffs(weeklyOffDaysCount, eventDetail.Id, command.WeekOffDays, place.Id);
                }
                else // if edit
                {
                    if (place.EventTypeId != command.PlaceType)
                    {
                        place.EventTypeId = command.PlaceType;
                        _eventRepository.Save(place);
                    }
                    index = 0;
                    var eventDetails = _eventDetailRepository.GetAllByEventId(place.Id).ToList();
                    Guid guid = Guid.NewGuid();
                    var eventDetail = _eventDetailRepository.GetAllByEventId(place.Id).ElementAt(index);
                    eventDetail.Name = place.Name;
                    eventDetail.StartDateTime = From;
                    eventDetail.EndDateTime = To;
                    eventDetail.IsEnabled = true;
                    _eventDetailRepository.Save(eventDetail);
                    deleteCalendar(place);
                    addCalendar(command, place);
                    var weeklyOffDaysCount = command.WeekOffDays.Where(s => s).Count();
                    UpdatePlaceWeekOff(weeklyOffDaysCount, eventDetail.Id, command.WeekOffDays, place.Id);
                    if (command.HolidayDates.Count() > 0)
                    {
                        UpdatePlaceHoliday(command.HolidayDates, eventDetail.Id, place.Id);
                    }
                    updateEventStatus(place.Id, false);
                }
                return new PlaceCalendarCommandResult
                {
                    Success = true,
                };
            }
            catch (Exception e)
            {
                return new PlaceCalendarCommandResult { };
            }
        }
    }
}