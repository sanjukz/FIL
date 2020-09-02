using FIL.Api.Repositories;
using FIL.Contracts.Enums;
using FIL.Contracts.Queries.EventSchedule;
using FIL.Contracts.QueryResults.EventSchedule;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.Events
{
    public class EventScheduleQueryHandler : IQueryHandler<EventScheduleQuery, EventScheduleQueryResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;

        public EventScheduleQueryHandler(IEventRepository eventRepository, IVenueRepository venueRepository,
            ITeamRepository teamRepository, ITicketCategoryRepository ticketCategoryRepository, ICurrencyTypeRepository currencyTypeRepository)
        {
            _eventRepository = eventRepository;
            _venueRepository = venueRepository;
            _teamRepository = teamRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _currencyTypeRepository = currencyTypeRepository;
        }

        public EventScheduleQueryResult Handle(EventScheduleQuery query)
        {
            var feetype = Enum.GetValues(typeof(Contracts.Enums.FeeType));
            var valuetype = Enum.GetValues(typeof(ValueTypes));

            List<FIL.Contracts.Models.FeeType> feeList = new List<FIL.Contracts.Models.FeeType>();
            List<FIL.Contracts.Models.ValueType> valueList = new List<FIL.Contracts.Models.ValueType>();

            foreach (var item in feetype)
            {
                FIL.Contracts.Models.FeeType fee = new Contracts.Models.FeeType();
                fee.Id = (int)item;
                fee.name = item.ToString();
                feeList.Add(fee);
            }
            foreach (var item in valuetype)
            {
                FIL.Contracts.Models.ValueType values = new Contracts.Models.ValueType();
                values.Id = (int)item;
                values.name = item.ToString();
                valueList.Add(values);
            }

            var eventDataModel = _eventRepository.GetAllByDateEvents(DateTime.UtcNow.AddDays(-90));

            var eventModel = AutoMapper.Mapper.Map<List<Contracts.Models.Event>>(eventDataModel);

            var venueDataModel = _venueRepository.GetAllVenues();
            var venues = AutoMapper.Mapper.Map<List<Contracts.Models.Venue>>(venueDataModel);

            var teamDataModel = _teamRepository.GetAll();
            var teams = AutoMapper.Mapper.Map<List<Contracts.Models.Team>>(teamDataModel);

            var ticketCategoriesDataModel = _ticketCategoryRepository.GetAll().GroupBy(o => o.Name).Select(o => o.FirstOrDefault());
            var ticketCategories = AutoMapper.Mapper.Map<List<Contracts.Models.TicketCategory>>(ticketCategoriesDataModel);

            var currencyTypeDataModel = _currencyTypeRepository.GetAll();
            var currencies = AutoMapper.Mapper.Map<List<Contracts.Models.CurrencyType>>(currencyTypeDataModel);

            return new EventScheduleQueryResult
            {
                Events = eventModel,
                Venues = venues,
                Teams = teams,
                FeeType = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.FeeType>>(feeList),
                ValueType = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.ValueType>>(valueList),
                TicketCategories = ticketCategories,
                Currencies = currencies
            };
        }
    }
}