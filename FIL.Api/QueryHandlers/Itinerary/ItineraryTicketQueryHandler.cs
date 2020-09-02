using FIL.Api.Repositories;
using FIL.Contracts.Queries.ItineraryTicket;
using FIL.Contracts.QueryResults.ItineraryTicket;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.Export
{
    public class ItineraryTicketQueryHandler : IQueryHandler<ItineraryTicketQuery, ItineraryTicketQueryResult>
    {
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;

        public ItineraryTicketQueryHandler(IEventDetailRepository eventDetailRepository, IEventTicketAttributeRepository eventTicketAttributeRepository, IEventTicketDetailRepository eventTicketDetailRepository, ITicketCategoryRepository ticketCategoryRepository)
        {
            _eventDetailRepository = eventDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
        }

        public ItineraryTicketQueryResult Handle(ItineraryTicketQuery query)
        {
            ItineraryTicketQueryResult itineraryTicketQueryResult = new ItineraryTicketQueryResult();
            foreach (Int64 eventId in query.eventIds)
            {
                ItineraryTicketDetails itineraryTicketDetails = new ItineraryTicketDetails();

                itineraryTicketDetails.eventId = eventId;

                List<Contracts.DataModels.EventDetail> eventDetail = _eventDetailRepository.GetAllByEventId(eventId).Where(u => u.IsEnabled == true).Distinct().ToList();

                List<Contracts.DataModels.EventTicketDetail> eventTicketDetail = _eventTicketDetailRepository.GetAllByEventDetailIds(eventDetail.Select(u => u.Id).Distinct().ToList()).Where(u => u.IsEnabled == true).Distinct().ToList();

                List<Contracts.DataModels.EventTicketAttribute> eventTicketAttribute = _eventTicketAttributeRepository.GetByEventTicketDetailIds(eventTicketDetail.Select(u => u.Id).ToList()).Distinct().ToList();

                List<Contracts.DataModels.TicketCategory> ticketCategory = _ticketCategoryRepository.GetByEventDetailIds(eventTicketDetail.Select(u => u.TicketCategoryId).ToList()).Distinct().ToList();

                itineraryTicketDetails.eventDetails = AutoMapper.Mapper.Map<List<Contracts.Models.EventDetails>>(eventDetail);
                itineraryTicketDetails.eventTicketDetails = AutoMapper.Mapper.Map<List<Contracts.Models.EventTicketDetail>>(eventTicketDetail);
                itineraryTicketDetails.eventTicketAttributes = AutoMapper.Mapper.Map<List<Contracts.Models.EventTicketAttribute>>(eventTicketAttribute);
                itineraryTicketDetails.ticketCategories = AutoMapper.Mapper.Map<List<Contracts.Models.TicketCategory>>(ticketCategory);

                itineraryTicketQueryResult.itineraryTicketDetails.Add(itineraryTicketDetails);
            }

            return itineraryTicketQueryResult;
        }
    }
}