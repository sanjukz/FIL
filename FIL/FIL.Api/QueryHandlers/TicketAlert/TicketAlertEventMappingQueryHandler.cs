using FIL.Api.Repositories;
using FIL.Contracts.Queries.TicketAlert;
using FIL.Contracts.QueryResults;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.Search
{
    public class TicketAlertEventMappingQueryHandler : IQueryHandler<TicketAlertQuery, TicketAlertQueryResult>
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly ITicketAlertEventMappingRepository _ticketAlertEventMappingRepository;

        public TicketAlertEventMappingQueryHandler(ICountryRepository countryRepository,
            ITicketAlertEventMappingRepository ticketAlertEventMappingRepository,
            IEventDetailRepository eventDetailRepository,
            IEventRepository eventRepository)
        {
            _countryRepository = countryRepository;
            _eventDetailRepository = eventDetailRepository;
            _eventRepository = eventRepository;
            _ticketAlertEventMappingRepository = ticketAlertEventMappingRepository;
        }

        public TicketAlertQueryResult Handle(TicketAlertQuery query)
        {
            var eventDataModel = _eventRepository.GetByAltId(query.altId);
            var eventModel = AutoMapper.Mapper.Map<FIL.Contracts.Models.Event>(eventDataModel);

            if (eventModel != null)
            {
                var ticketAlertEventMappingDataModel = _ticketAlertEventMappingRepository.GetByEventId(eventModel.Id);
                var ticketAlertEventMappingModel = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.TicketAlertEventMapping>>(ticketAlertEventMappingDataModel);

                var eventDetailDataModel = _eventDetailRepository.GetByEventDetailIds(ticketAlertEventMappingModel.Select(s => s.EventDetailId));
                var eventDetailModel = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.EventDetail>>(eventDetailDataModel);

                var countriesDataModel = _countryRepository.GetByCountryIds(ticketAlertEventMappingModel.Select(s => s.CountryId));
                var countriesModel = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.Country>>(countriesDataModel);

                var AllCountries = _countryRepository.GetAll().OrderBy(o => o.Name);
                var allCountriesModel = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.Country>>(AllCountries);

                return new TicketAlertQueryResult
                {
                    Event = eventModel,
                    Countries = countriesModel,
                    AllCountries = allCountriesModel,
                    EventDetails = eventDetailModel,
                    ticketAlertEventMappings = ticketAlertEventMappingModel
                };
            }
            else
            {
                return new TicketAlertQueryResult
                {
                    Event = eventModel
                };
            }
        }
    }
}