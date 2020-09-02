using FIL.Api.Repositories;
using FIL.Contracts.Queries.EventSchedule;
using FIL.Contracts.QueryResults.EventSchedule;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.Events
{
    public class EditEventScheduleQueryHandler : IQueryHandler<EditEventScheduleQuery, EditEventScheduleQueryResult>
    {
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventDeliveryTypeDetailRepository _eventDeliveryTypeDetailRepository;
        private readonly IMatchAttributeRepository _matchAttributeRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ITicketFeeDetailRepository _ticketFeeDetailRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;

        public EditEventScheduleQueryHandler(IEventDetailRepository eventDetailRepository, IEventDeliveryTypeDetailRepository eventDeliveryTypeDetailRepository, IMatchAttributeRepository matchAttributeRepository,
            IEventTicketDetailRepository eventTicketDetailRepository, IEventTicketAttributeRepository eventTicketAttributeRepository,
            ITicketFeeDetailRepository ticketFeeDetailRepository, ITicketCategoryRepository ticketCategoryRepository)
        {
            _eventDetailRepository = eventDetailRepository;
            _eventDeliveryTypeDetailRepository = eventDeliveryTypeDetailRepository;
            _matchAttributeRepository = matchAttributeRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _ticketFeeDetailRepository = ticketFeeDetailRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
        }

        public EditEventScheduleQueryResult Handle(EditEventScheduleQuery query)
        {
            var eventDetailDataModel = _eventDetailRepository.GetByEvent(query.EventId);
            var deliveryTypeDetails = _eventDeliveryTypeDetailRepository.GetAllActivatedByEventDetailIds(eventDetailDataModel.Select(s => s.Id));
            var matchAttributes = _matchAttributeRepository.GetByEventDetailIds(eventDetailDataModel.Select(s => s.Id));
            var eventTicketDetails = _eventTicketDetailRepository.GetByEventDetailIds(eventDetailDataModel.Select(s => s.Id));
            var eventTicketAttributes = _eventTicketAttributeRepository.GetByEventTicketDetailIds(eventTicketDetails.Select(s => s.Id));
            var ticketFeeDetails = _ticketFeeDetailRepository.GetAllEnabledByEventTicketAttributeIds(eventTicketAttributes.Select(s => s.Id));

            var eventDetailsModel = AutoMapper.Mapper.Map<List<Contracts.Models.EventDetail>>(eventDetailDataModel);
            var deliveryTypeDetailsModel = AutoMapper.Mapper.Map<List<Contracts.Models.EventDeliveryTypeDetail>>(deliveryTypeDetails);
            var matchAttributesModel = AutoMapper.Mapper.Map<List<Contracts.Models.MatchAttribute>>(matchAttributes);
            var eventTicketDetailsModel = AutoMapper.Mapper.Map<List<Contracts.Models.EventTicketDetail>>(eventTicketDetails);
            var eventTicketAttributesModel = AutoMapper.Mapper.Map<List<Contracts.Models.EventTicketAttribute>>(eventTicketAttributes);
            var ticketFeeDetailsModel = AutoMapper.Mapper.Map<List<Contracts.Models.TicketFeeDetail>>(ticketFeeDetails);

            return new EditEventScheduleQueryResult
            {
                SubEvents = eventDetailsModel,
                DeliveryTypeDetails = deliveryTypeDetailsModel,
                MatchAttributes = matchAttributesModel,
                EventTicketDetails = eventTicketDetailsModel,
                EventTicketAttributes = eventTicketAttributesModel,
                TicketFeeDetails = ticketFeeDetailsModel
            };
        }
    }
}