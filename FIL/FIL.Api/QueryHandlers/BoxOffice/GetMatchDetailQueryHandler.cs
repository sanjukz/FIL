using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.BoxOffice;
using FIL.Contracts.QueryResults.BoxOffice;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.BoxOffice
{
    public class GetMatchDetailQueryHandler : IQueryHandler<GetMatchDetailQuery, GetMatchDetailQueryResult>
    {
        private readonly ITransactionDetailRepository _transactionDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;

        public GetMatchDetailQueryHandler(ITransactionDetailRepository transactionDetailRepository, IEventTicketAttributeRepository eventTicketAttributeRepository, IEventTicketDetailRepository eventTicketDetailRepository, IEventDetailRepository eventDetailRepository, ITicketCategoryRepository ticketCategoryRepository)
        {
            _transactionDetailRepository = transactionDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventDetailRepository = eventDetailRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
        }

        public GetMatchDetailQueryResult Handle(GetMatchDetailQuery query)
        {
            var transactionDetails = _transactionDetailRepository.GetByTransactionId(query.TransactionId);
            var eventTicketAttributes = _eventTicketAttributeRepository.GetByEventTicketAttributeIds(transactionDetails.Select(cb => cb.EventTicketAttributeId).Distinct());
            var eventTicketDetailDataModel = _eventTicketDetailRepository.GetByEventTicketDetailIds(eventTicketAttributes.Select(eta => eta.EventTicketDetailId)).Where(w => w.IsEnabled).Distinct().ToDictionary(etd => etd.Id);
            var eventDetailsDataModel = _eventDetailRepository.GetByIds(eventTicketDetailDataModel.Values.Select(etd => etd.EventDetailId).Distinct()).ToDictionary(ed => ed.Id);
            var eventTicketCategoryDataModel = _ticketCategoryRepository.GetByTicketCategoryIds(eventTicketDetailDataModel.Values.Select(etd => etd.TicketCategoryId).Distinct()).ToDictionary(tc => tc.Id);

            var getMatchDetailContainer = eventTicketAttributes.Select(cb =>
            {
                var eventTicketDetail = eventTicketDetailDataModel[cb.EventTicketDetailId];
                var ticketcategories = eventTicketCategoryDataModel[(int)eventTicketDetail.TicketCategoryId];
                var eventDetails = eventDetailsDataModel[eventTicketDetail.EventDetailId];

                return new GetMatchDetailContainer
                {
                    EventDetail = AutoMapper.Mapper.Map<EventDetail>(eventDetails),
                    TicketCategory = AutoMapper.Mapper.Map<FIL.Contracts.Models.TicketCategory>(ticketcategories)
                };
            });
            return new GetMatchDetailQueryResult
            {
                GetMatchDetailContainer = getMatchDetailContainer.ToList()
            };
        }
    }
}