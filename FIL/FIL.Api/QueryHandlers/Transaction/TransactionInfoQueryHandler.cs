using FIL.Api.Repositories;
using FIL.Contracts.Enums;
using FIL.Contracts.Queries.Transaction;
using FIL.Contracts.QueryResults.Transaction;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.Transactions
{
    public class TransactionInfoQueryHandler : IQueryHandler<TransactionInfoQuery, TransactionInfoQueryResult>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionDetailRepository _transactionDetailsRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventAttributeRepository _eventAttributeRepository;
        private readonly IEventRepository _eventRepository;
        private readonly ICurrencyTypeRepository _currencyType;

        public TransactionInfoQueryHandler(ICurrencyTypeRepository currencyType,
            ITransactionRepository transactionRepository,
             ITransactionDetailRepository transactionDetailsRepository,
        IEventTicketDetailRepository eventTicketDetailRepository,
        ITicketCategoryRepository ticketCategoryRepository,
        IEventDetailRepository eventDetailRepository,
        IEventAttributeRepository eventAttributeRepository,
        IEventTicketAttributeRepository eventTicketAttributeRepository,
        IEventRepository eventRepository)
        {
            _currencyType = currencyType;
            _transactionRepository = transactionRepository;
            _transactionDetailsRepository = transactionDetailsRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _eventDetailRepository = eventDetailRepository;
            _eventAttributeRepository = eventAttributeRepository;
            _eventRepository = eventRepository;
        }

        public TransactionInfoQueryResult Handle(TransactionInfoQuery query)
        {
            var transaction = AutoMapper.Mapper.Map<Contracts.Models.Transaction>(_transactionRepository.GetByTransactionAltId(query.TransactionAltId));
            TransactionType transactionType = TransactionType.Regular;
            var transactionModel = AutoMapper.Mapper.Map<FIL.Contracts.Models.Transaction>(transaction);
            var transactionDetails = _transactionDetailsRepository.GetByTransactionId(transactionModel.Id);
            var transactionDetailModel = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.TransactionDetail>>(transactionDetails);
            if (transactionDetails.FirstOrDefault().TransactionType == TransactionType.QRCode)
            {
                transactionType = TransactionType.QRCode;
            }
            var eventTicketAttributeDetails = AutoMapper.Mapper.Map<List<Contracts.Models.EventTicketAttribute>>(_eventTicketAttributeRepository.GetByEventTicketAttributeIds(transactionDetails.Select(s => s.EventTicketAttributeId)));

            var eventTicketDetails = _eventTicketDetailRepository.GetByEventTicketDetailIds(eventTicketAttributeDetails.Select(s => s.EventTicketDetailId));

            var eventDetails = _eventDetailRepository.GetByEventDetailIds(eventTicketDetails.Select(s => s.EventDetailId).Distinct());

            IEnumerable<FIL.Contracts.DataModels.Event> events = new List<FIL.Contracts.DataModels.Event>();
            events = _eventRepository.GetByAllTypeEventIds(eventDetails.Select(s => s.EventId).Distinct());

            var currency = _currencyType.Get(transaction.CurrencyId);

            return new TransactionInfoQueryResult
            {
                Transaction = transaction,
                CurrencyName = currency.Code,
                TransactionType = transactionType,
                Events = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.Event>>(events),
                EventTicketAttributes = eventTicketAttributeDetails,
                TransactionDetails = transactionDetailModel
            };
        }
    }
}