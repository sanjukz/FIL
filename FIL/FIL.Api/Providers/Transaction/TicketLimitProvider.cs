using FIL.Api.Repositories;
using FIL.Configuration;
using FIL.Contracts.Commands.Transaction;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Providers.Transaction
{
    public interface ITicketLimitProvider
    {
        CheckoutCommandResult CheckTicketLimit(CheckoutCommand checkoutCommand);
    }

    public class TicketLimitProvider : ITicketLimitProvider
    {
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionDetailRepository _transactionDetailRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly FIL.Logging.ILogger _logger;

        public TicketLimitProvider(ILogger logger, ISettings settings,
                 IEventTicketAttributeRepository eventTicketAttributeRepository,
                 ITransactionRepository transactionRepository,
                 ITransactionDetailRepository transactionDetailRepository,
            IEventDetailRepository eventDetailRepository,
            IEventTicketDetailRepository eventTicketDetailRepository
            )
        {
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _transactionRepository = transactionRepository;
            _transactionDetailRepository = transactionDetailRepository;
            _eventDetailRepository = eventDetailRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _logger = logger;
        }

        public CheckoutCommandResult CheckTicketLimit(CheckoutCommand checkoutCommand)
        {
            try
            {
                FIL.Contracts.Models.EventDetail eventDetail = new Contracts.Models.EventDetail();
                bool isPurchase = false;
                List<FIL.Contracts.DataModels.Transaction> transactionByData = new List<Contracts.DataModels.Transaction>();
                if (checkoutCommand.GuestUser != null && checkoutCommand.GuestUser.Email != null && checkoutCommand.GuestUser.PhoneNumber != null)
                {
                    transactionByData = _transactionRepository.GetSuccessFullTransactionByEmailOrPhoneNumber(checkoutCommand.GuestUser.Email, checkoutCommand.GuestUser.PhoneNumber).Where(s => s.ChannelId == Channels.Website).ToList();
                }
                if (transactionByData.Count() > 0 && !(bool)checkoutCommand.IsASI)
                {
                    IEnumerable<TransactionDetail> transactionDetails = _transactionDetailRepository.GetByTransactionIds(transactionByData.Select(s => s.Id).Distinct());
                    IEnumerable<Contracts.DataModels.EventTicketAttribute> eventTicketAttributeDetails = _eventTicketAttributeRepository.GetByEventTicketAttributeIds(transactionDetails.Select(s => s.EventTicketAttributeId).Distinct());
                    IEnumerable<Contracts.DataModels.EventTicketDetail> eventTicketDetails = _eventTicketDetailRepository.GetByEventTicketDetailIds(eventTicketAttributeDetails.Select(s => s.EventTicketDetailId));
                    IEnumerable<EventDetail> eventDetails = _eventDetailRepository.GetByEventDetailIds(eventTicketDetails.Select(s => s.EventDetailId).Distinct());

                    foreach (FIL.Contracts.DataModels.EventDetail itemEventDetail in eventDetails)
                    {
                        var filteredOrderEventTicketAttribute = checkoutCommand.EventTicketAttributeList.Where(s => s.EventDetailId == itemEventDetail.Id);
                        if (filteredOrderEventTicketAttribute.Any())
                        {
                            var tEventTicketDetails = eventTicketDetails.Where(w => w.EventDetailId == itemEventDetail.Id).Distinct();
                            var tETD = eventTicketDetails.Where(s => s.EventDetailId == itemEventDetail.Id);
                            var tETA = eventTicketAttributeDetails.Where(w => tETD.Select(s => s.Id).Contains(w.EventTicketDetailId));
                            int transactionTotalTickets = transactionDetails.Where(w => tETA.Select(s => s.Id).Contains(w.EventTicketAttributeId)).Distinct().Select(s => s.TotalTickets).Sum(item => item);
                            int commanEventTicketAttributesSum = filteredOrderEventTicketAttribute.Select(s => s.TotalTickets).Sum(item => item);
                            int ticketLimit = 10;
                            if (itemEventDetail.TicketLimit != null)
                            {
                                ticketLimit = (int)itemEventDetail.TicketLimit;
                            }
                            if ((commanEventTicketAttributesSum + transactionTotalTickets) > ticketLimit)
                            {
                                eventDetail = AutoMapper.Mapper.Map<FIL.Contracts.Models.EventDetail>(itemEventDetail);
                                isPurchase = true;
                                break;
                            }
                        }
                    }
                }
                if (isPurchase && !checkoutCommand.ISRasv)
                {
                    return new CheckoutCommandResult
                    {
                        Id = 0,
                        Success = false,
                        EventName = eventDetail.Name,
                        IsTransactionLimitExceed = true,
                        IsTicketCategorySoldOut = false
                    };
                }
                else
                {
                    return new CheckoutCommandResult
                    {
                        Success = true
                    };
                }
            }
            catch (Exception e)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, e);
                return new CheckoutCommandResult
                {
                    Success = true
                };
            }
        }
    }
}