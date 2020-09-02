using FIL.Api.Providers;
using FIL.Api.Repositories;
using FIL.Api.Repositories.Tiqets;
using FIL.Contracts.DataModels;
using FIL.Contracts.Models.Zoom;
using FIL.Contracts.Queries.OrderConfirmation;
using FIL.Contracts.QueryResults.OrderConfirmation;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.OrderConfirmation
{
    public class OrderConfirmationQueryHandler : IQueryHandler<OrderConfirmationQuery, OrderConfirmationQueryResult>
    {
        private IOrderConfirmationProvider _orderConfirmationProvider;
        private readonly ITiqetsTransactionRepository _tiqetsTransactionRepository;
        private readonly IZoomUserRepository _zoomUserRepository;
        private readonly IEventHostMappingRepository _eventHostMappingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEventRepository _eventRepository;
        private readonly ITransactionRepository _transactionRepository;

        public OrderConfirmationQueryHandler(IOrderConfirmationProvider orderConfirmationProvider,
            ITiqetsTransactionRepository tiqetsTransactionRepository,
            IZoomUserRepository zoomUserRepository,
            IUserRepository userRepository,
            IEventHostMappingRepository eventHostMappingRepository,
            IEventRepository eventRepository, ITransactionRepository transactionRepository)
        {
            _orderConfirmationProvider = orderConfirmationProvider;
            _tiqetsTransactionRepository = tiqetsTransactionRepository;
            _transactionRepository = transactionRepository;
            _zoomUserRepository = zoomUserRepository;
            _userRepository = userRepository;
            _eventHostMappingRepository = eventHostMappingRepository;
            _eventRepository = eventRepository;
        }

        public OrderConfirmationQueryResult Handle(OrderConfirmationQuery query)
        {
            var orderConfirmation = _orderConfirmationProvider.Get(query.TransactionId, false, Contracts.Enums.Channels.Website);
            if (orderConfirmation != null)
            {
                if (orderConfirmation.orderConfirmationSubContainer[0].Event.AltId.ToString().ToUpper() == "1F0257FA-EEA6-4469-A7BC-B878A215C8A9")
                {
                    orderConfirmation.Transaction.CreatedUtc = orderConfirmation.Transaction.CreatedUtc.AddHours(10);
                }

                // Check if transaction is Tiqets One
                var tiqetsTransactions = _tiqetsTransactionRepository.GetByTransactionId(query.TransactionId);

                //for live online events
                var zoomUserModel = new ZoomUser();
                List<ZoomHostUserModel> hostUsersModel = new List<ZoomHostUserModel>();
                var liveOnlineDetailModel = _transactionRepository.GetFeelOnlineDetails(query.TransactionId).FirstOrDefault();
                if (liveOnlineDetailModel != null)
                {
                    //check if subcategory is LiveOnline
                    var eventModel = _eventRepository.Get(liveOnlineDetailModel.EventId);
                    if (eventModel.EventCategoryId == 119)
                    {
                        zoomUserModel = _zoomUserRepository.GetByTransactionId(query.TransactionId);

                        var transactionCount = _transactionRepository.GetTransactionCountByEvent(liveOnlineDetailModel.EventId);
                        if (transactionCount == 1)
                        {
                            var eventHostUsersModel = _eventHostMappingRepository.GetAllByEventId(liveOnlineDetailModel.EventId);
                            var zoomHostUserModel = _zoomUserRepository.GetByHostUserIds(eventHostUsersModel.Select(s => s.Id));
                            foreach (var currentZoomUser in zoomHostUserModel)
                            {
                                var currentHostUsersModel = new ZoomHostUserModel();
                                var currentEventHost = eventHostUsersModel.Where(s => s.Id == currentZoomUser.EventHostUserId).FirstOrDefault();
                                currentHostUsersModel.altId = currentZoomUser.AltId;
                                currentHostUsersModel.email = currentEventHost.Email;
                                hostUsersModel.Add(currentHostUsersModel);
                            }
                        }
                    }
                }
                return new OrderConfirmationQueryResult
                {
                    Transaction = orderConfirmation.Transaction,
                    TransactionPaymentDetail = orderConfirmation.TransactionPaymentDetail,
                    UserCardDetail = orderConfirmation.UserCardDetail,
                    CurrencyType = orderConfirmation.CurrencyType,
                    PaymentOption = orderConfirmation.PaymentOption,
                    cardTypes = orderConfirmation.cardTypes,
                    orderConfirmationSubContainer = orderConfirmation.orderConfirmationSubContainer,
                    TicketQuantity = orderConfirmation.TicketQuantity,
                    GoodsAndServiceTax = orderConfirmation.GoodsAndServiceTax,
                    IsASI = orderConfirmation.IsASI,
                    IsTiqets = tiqetsTransactions != null ? true : false,
                    liveOnlineDetailModel = liveOnlineDetailModel,
                    ZoomUser = zoomUserModel,
                    hostUsersModel = hostUsersModel
                };
            }
            else
            {
                return new OrderConfirmationQueryResult
                {
                    Transaction = null,
                    TransactionPaymentDetail = null,
                    UserCardDetail = null,
                    CurrencyType = null,
                    PaymentOption = null,
                    cardTypes = null,
                    orderConfirmationSubContainer = null,
                    IsASI = false,
                    IsTiqets = false
                };
            }
        }
    }
}