using FIL.Api.Repositories;
using FIL.Contracts.Queries.ReviewsAndRating;
using FIL.Contracts.QueryResults.ReviewsAndRating;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.ReviewsAndRating
{
    public class ReviewsAndRatingQueryHandler : IQueryHandler<ReviewsAndRatingQuery, ReviewsAndRatingQueryResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionDetailRepository _transactionDetailRepository;
        private readonly ITransactionPaymentDetailRepository _transactionPaymentDetailRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;

        public ReviewsAndRatingQueryHandler(IUserRepository userRepository,
            IEventRepository eventRepository,
            IEventDetailRepository eventDetailRepository,
            IEventTicketAttributeRepository eventTicketAttributeRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            ITicketCategoryRepository ticketCategoryRepository,
            ITransactionRepository transactionRepository,
            ITransactionDetailRepository transactionDetailRepository,
            ITransactionPaymentDetailRepository transactionPaymentDetailRepository,
            ICurrencyTypeRepository currencyTypeRepository)
        {
            _userRepository = userRepository;
            _eventRepository = eventRepository;
            _eventDetailRepository = eventDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _transactionRepository = transactionRepository;
            _transactionDetailRepository = transactionDetailRepository;
            _transactionPaymentDetailRepository = transactionPaymentDetailRepository;
            _currencyTypeRepository = currencyTypeRepository;
        }

        public ReviewsAndRatingQueryResult Handle(ReviewsAndRatingQuery query)
        {
            var userDataModel = _userRepository.GetByAltId(query.UserAltId);
            bool isPurchased = false;
            if (userDataModel.Email.Contains("feelaplace"))
            {
                isPurchased = true;
            }
            var transactionModel = _transactionRepository.GetByUserAltId(query.UserAltId);

            var transactionDetailmodel = _transactionDetailRepository.GetByTransactionIds(transactionModel.Select(s => s.Id));
            var transDetailModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.TransactionDetail>>(transactionDetailmodel);

            var eventTicketAttributeModel = _eventTicketAttributeRepository.GetByEventTicketAttributeIds(transactionDetailmodel.Select(s => s.EventTicketAttributeId));
            var eventAttributeTicketModel = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.EventTicketAttribute>>(eventTicketAttributeModel);

            var eventticketDetailModel = _eventTicketDetailRepository.GetByEventTicketDetailIds(eventTicketAttributeModel.Select(s => s.EventTicketDetailId));
            var eventticketDetailModels = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.EventTicketDetail>>(eventticketDetailModel);

            var ticketCategorymodel = _ticketCategoryRepository.GetByTicketCategoryIds(eventticketDetailModel.Select(s => s.TicketCategoryId));
            var ticketCategorymodel1 = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.TicketCategory>>(ticketCategorymodel);

            var eventDetailModel = _eventDetailRepository.GetByEventDetailIds(eventticketDetailModel.Select(s => s.EventDetailId));

            var eventmodel = _eventRepository.GetByEventIds(eventDetailModel.Select(s => s.EventId));
            var eventmodel1 = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.Event>>(eventmodel);

            if (transactionModel.ToList().Count > 0)
            {
                isPurchased = true;
            }
            return new ReviewsAndRatingQueryResult
            {
                IsPurchase = isPurchased
            };
        }
    }
}