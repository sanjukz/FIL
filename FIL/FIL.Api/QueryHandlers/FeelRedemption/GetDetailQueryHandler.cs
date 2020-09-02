using FIL.Api.Repositories;
using FIL.Contracts.Queries.FeelRedemption;
using FIL.Contracts.QueryResults.FeelRedemption;
using FIL.Logging;
using FIL.Logging.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.Users
{
    public class GetDetailQueryHandler : IQueryHandler<GetDetailQuery, GetDetailQueryResult>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionDetailRepository _transactionDetailsRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventRepository _eventRepository;
        private readonly ILogger _logger;

        public GetDetailQueryHandler(ITransactionRepository transactionRepository,
            ITransactionDetailRepository transactionDetailsRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            ITicketCategoryRepository ticketCategoryRepository,
            IEventDetailRepository eventDetailRepository,
            IEventTicketAttributeRepository eventTicketAttributeRepository,
            IEventRepository eventRepository, ILogger logger)
        {
            _transactionRepository = transactionRepository;
            _transactionDetailsRepository = transactionDetailsRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _eventDetailRepository = eventDetailRepository;
            _eventRepository = eventRepository;
            _logger = logger;
        }

        public GetDetailQueryResult Handle(GetDetailQuery query)
        {
            try
            {
                var transaction = _transactionRepository.GetSuccessfulTransactionDetails(query.TransactionId);
                if (transaction == null)
                {
                    return new GetDetailQueryResult();
                }

                var transactionDetails = _transactionDetailsRepository.GetByTransactionId(transaction.Id).ToList();
                var eventTicketAttributes = _eventTicketAttributeRepository.GetByEventTicketAttributeIds(transactionDetails.Select(s => s.EventTicketAttributeId).Distinct());
                var eventTicketDetails = _eventTicketDetailRepository.GetByIds(eventTicketAttributes.Select(s => s.EventTicketDetailId).Distinct());
                var eventDetails = _eventDetailRepository.GetByEventDetailIds(eventTicketDetails.Select(s => s.EventDetailId).Distinct());
                var events = _eventRepository.GetByAllEventIds(eventDetails.Select(s => s.EventId).Distinct());
                var filteredEvents = events.Where(s => s.CreatedBy == query.UserAltId);
                var filteredEventDetails = eventDetails
                   .Where(x => filteredEvents.Any(y => y.Id == x.EventId));
                var filteredEventTicketDetails = eventTicketDetails
                   .Where(x => filteredEventDetails.Any(y => y.Id == x.EventDetailId));
                var ticketCategories = _ticketCategoryRepository.GetAllTicketCategory(filteredEventTicketDetails.Select(s => s.TicketCategoryId));
                var filteredEventTicktAttributes = eventTicketAttributes
                   .Where(x => filteredEventTicketDetails.Any(y => y.Id == x.EventTicketDetailId));
                var filteredTransactionDetail = transactionDetails
                   .Where(x => filteredEventTicktAttributes.Any(y => y.Id == x.EventTicketAttributeId));
                return new GetDetailQueryResult
                {
                    EventDetail = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.EventDetail>>(filteredEventDetails),
                    EventTicketAttribute = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.EventTicketAttribute>>(filteredEventTicktAttributes),
                    TicketCategory = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.TicketCategory>>(ticketCategories),
                    EventTicketDetail = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.EventTicketDetail>>(eventTicketDetails),
                    TransactionDetail = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.TransactionDetail>>(filteredTransactionDetail)
                };
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to get Details", e));
                return new GetDetailQueryResult();
            }
        }
    }
}