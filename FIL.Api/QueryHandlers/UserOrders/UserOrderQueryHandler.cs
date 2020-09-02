using AutoMapper;
using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.UserOrders;
using FIL.Contracts.QueryResults.UserOrder;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.UserOrders
{
    public class UserOrderQueryHandler : IQueryHandler<UserOrdersQuery, UserOrderQueryResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionDetailRepository _transactionDetailRepository;
        private readonly ITransactionPaymentDetailRepository _transactionPaymentDetailRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;
        private readonly IEventCategoryRepository _eventCategoryRepository;
        private readonly IEventCategoryMappingRepository _eventCategoryMappingRepository;

        public UserOrderQueryHandler(IEventRepository eventRepository,
            IEventDetailRepository eventDetailRepository,
            IEventTicketAttributeRepository eventTicketAttributeRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            ITicketCategoryRepository ticketCategoryRepository,
            ITransactionRepository transactionRepository,
            ITransactionDetailRepository transactionDetailRepository,
            ITransactionPaymentDetailRepository transactionPaymentDetailRepository,
            ICurrencyTypeRepository currencyTypeRepository,
            IEventCategoryRepository eventCategoryRepository,
            IEventCategoryMappingRepository eventCategoryMappingRepository
        )

        {
            _eventRepository = eventRepository;
            _eventDetailRepository = eventDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _transactionRepository = transactionRepository;
            _transactionDetailRepository = transactionDetailRepository;
            _transactionPaymentDetailRepository = transactionPaymentDetailRepository;
            _currencyTypeRepository = currencyTypeRepository;
            _eventCategoryRepository = eventCategoryRepository;
            _eventCategoryMappingRepository = eventCategoryMappingRepository;
        }

        public UserOrderQueryResult Handle(UserOrdersQuery query)
        {
            var transactionModel = _transactionRepository.GetByUserAltId(query.UserAltId);
            var transModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.Transaction>>(transactionModel);

            var transactionDetailmodel = _transactionDetailRepository.GetByTransactionIds(transactionModel.Select(s => s.Id));
            var transDetailModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.TransactionDetail>>(transactionDetailmodel);

            var eventTicketAttributeModel = _eventTicketAttributeRepository.GetByEventTicketAttributeIds(transactionDetailmodel.Select(s => s.EventTicketAttributeId));
            var eventAttributeTicketModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.EventTicketAttribute>>(eventTicketAttributeModel);

            var eventticketDetailModel = _eventTicketDetailRepository.GetByEventTicketDetailIds(eventTicketAttributeModel.Select(s => s.EventTicketDetailId));
            var eventticketDetailModels = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.EventTicketDetail>>(eventticketDetailModel);

            var ticketCategorymodel = _ticketCategoryRepository.GetByTicketCategoryIds(eventticketDetailModel.Select(s => s.TicketCategoryId));
            var ticketCategorymodel1 = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.TicketCategory>>(ticketCategorymodel);

            var eventDetailModel = _eventDetailRepository.GetByEventDetailIds(eventticketDetailModel.Select(s => s.EventDetailId));
            var eventDetailModel1 = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.EventDetail>>(eventDetailModel);

            var eventmodel = _eventRepository.GetByAllTypeEventIds(eventDetailModel.Select(s => s.EventId));
            var eventmodel1 = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.Event>>(eventmodel);

            var transactionpaymentDetail = _transactionPaymentDetailRepository.GetByTransactionIds(transactionModel.Select(s => s.Id));
            var transactionpaymentmodel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.TransactionPaymentDetail>>(transactionpaymentDetail);

            var currencyTypeData = transactionModel.Select(item =>
            {
                var tCurrenyTypeDetail = _currencyTypeRepository.GetByCurrencyId(item.CurrencyId);
                var tCurrenyTypeDetailModel = AutoMapper.Mapper.Map<Contracts.Models.CurrencyType>(tCurrenyTypeDetail);

                return new CurrencyTypeContainer
                {
                    CurrencyType = Mapper.Map<CurrencyType>(tCurrenyTypeDetailModel)
                };
            });

            var eventCategoryMappings = _eventCategoryMappingRepository.GetByEventIds(eventmodel.Select(s => s.Id));
            var eventCategoryMappings1 = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.EventCategoryMapping>>(eventCategoryMappings);
            var eventCategory = _eventCategoryRepository.GetByIds(eventCategoryMappings.Select(s => s.EventCategoryId));
            var eventCategory1 = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.EventCategory>>(eventCategory);

            return new UserOrderQueryResult
            {
                Event = eventmodel1,
                EventTicketAttribute = eventAttributeTicketModel,
                EventTicketDetail = eventticketDetailModels,
                TicketCategory = ticketCategorymodel1,
                Transaction = transModel,
                transactionDetail = transDetailModel,
                TransactionPaymentDetail = transactionpaymentmodel,
                CurrencyType = currencyTypeData.ToList(),
                EventDetail = eventDetailModel1,
                EventCategories = eventCategory1,
                EventCategoryMappings = eventCategoryMappings1
            };
        }
    }
}