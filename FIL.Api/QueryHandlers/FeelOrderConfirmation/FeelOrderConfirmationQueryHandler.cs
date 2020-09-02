using FIL.Api.Providers;
using FIL.Api.Repositories;
using FIL.Api.Repositories.Tiqets;
using FIL.Contracts.DataModels;
using FIL.Contracts.Models.Zoom;
using FIL.Contracts.Queries.FeelOrderConfirmation;
using FIL.Contracts.QueryResults.FeelOrderConfirmation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.FeelOrderConfirmation
{
    public class FeelOrderConfirmationQueryHandler : IQueryHandler<FeelOrderConfirmationQuery, FeelOrderConfirmationQueryResult>
    {
        private IOrderConfirmationProvider _orderConfirmationProvider;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITiqetsTransactionRepository _tiqetsTransactionRepository;
        private readonly IZoomUserRepository _zoomUserRepository;
        private readonly IEventHostMappingRepository _eventHostMappingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEventCategoryRepository _eventCategoryRepository;

        public FeelOrderConfirmationQueryHandler(IOrderConfirmationProvider orderConfirmationProvider,
            ITransactionRepository transactionRepository,
            ITiqetsTransactionRepository tiqetsTransactionRepository,
            IZoomUserRepository zoomUserRepository,
            IUserRepository userRepository,
            IEventHostMappingRepository eventHostMappingRepository, IEventCategoryRepository eventCategoryRepository)
        {
            _orderConfirmationProvider = orderConfirmationProvider;
            _transactionRepository = transactionRepository;
            _tiqetsTransactionRepository = tiqetsTransactionRepository;
            _zoomUserRepository = zoomUserRepository;
            _userRepository = userRepository;
            _eventHostMappingRepository = eventHostMappingRepository;
            _eventCategoryRepository = eventCategoryRepository;
        }

        public FeelOrderConfirmationQueryResult Handle(FeelOrderConfirmationQuery query)
        {
            var transaction = _transactionRepository.GetByAltId(query.TransactionAltId);
            if (transaction == null)
            {
                return new FeelOrderConfirmationQueryResult
                { };
            }
            var orderConfirmation = _orderConfirmationProvider.Get(transaction.Id,
                query.confirmationFrmMyOrders,
                query.Channel);

            // Check if transaction is Tiqets One
            var tiqetsTransactions = _tiqetsTransactionRepository.GetByTransactionId(transaction.Id);

            //for live online events
            var IsLiveOnline = false;
            var zoomUserModel = new ZoomUser();
            var liveOnlineDetailModelData = new LiveOnlineTransactionDetailResponseModel();
            List<ZoomHostUserModel> hostUsersModel = new List<ZoomHostUserModel>();
            var liveOnlineDetailModel = _transactionRepository.GetFeelOnlineDetails(transaction.Id);
            if (liveOnlineDetailModel.Any()
                && liveOnlineDetailModel.Count() == 1
                && (liveOnlineDetailModel.Select(s => s.TicketCategoryId).Contains(19452) || liveOnlineDetailModel.Select(s => s.TicketCategoryId).Contains(12259)))
            {
                liveOnlineDetailModelData = liveOnlineDetailModel.FirstOrDefault();
            }
            else
            {
                liveOnlineDetailModelData = liveOnlineDetailModel.Where(s => s.TransactionType == Contracts.Enums.TransactionType.LiveOnline).FirstOrDefault();
            }
            if (liveOnlineDetailModelData != null && liveOnlineDetailModelData.EventcategoryId != 0)
            {
                if (orderConfirmation.orderConfirmationSubContainer.Any(s => s.Event.MasterEventTypeId == Contracts.Enums.MasterEventType.Online))
                {
                    IsLiveOnline = true;
                    zoomUserModel = _zoomUserRepository.GetByTransactionId(transaction.Id);
                    if (zoomUserModel != null)
                    {
                        var transactionCount = _transactionRepository.GetTransactionCountByEvent(liveOnlineDetailModelData.EventId);
                        if (transactionCount == 1)
                        {
                            var eventHostUsersModel = _eventHostMappingRepository.GetAllByEventId(liveOnlineDetailModelData.EventId);
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
            }

            return new FeelOrderConfirmationQueryResult
            {
                Transaction = orderConfirmation.Transaction,
                TransactionPaymentDetail = orderConfirmation.TransactionPaymentDetail,
                UserCardDetail = orderConfirmation.UserCardDetail,
                CurrencyType = orderConfirmation.CurrencyType,
                PaymentOption = orderConfirmation.PaymentOption,
                cardTypes = orderConfirmation.cardTypes,
                orderConfirmationSubContainer = orderConfirmation.orderConfirmationSubContainer,
                IsTiqets = tiqetsTransactions != null ? true : false,
                IsHoho = orderConfirmation.IsHoho,
                ZoomUser = zoomUserModel,
                liveOnlineDetailModel = liveOnlineDetailModelData,
                hostUsersModel = hostUsersModel,
                IsLiveOnline = IsLiveOnline
            };
        }
    }
}