using AutoMapper;
using FIL.Api.Repositories;
using FIL.Contracts.Enums;
using FIL.Contracts.Queries.ICCPaymentCheck;
using FIL.Contracts.QueryResults.ICCPaymentCheck;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.ICCPaymentCheck
{
    public class ICCPaymentCheckQueryHandler : Profile, IQueryHandler<ICCPaymentCheckQuery, ICCPaymentCheckQueryResult>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionDetailRepository _transactionDetailsRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventAttributeRepository _eventAttributeRepository;
        private readonly IEventRepository _eventRepository;

        public ICCPaymentCheckQueryHandler(ITransactionRepository transactionRepository,
        ITransactionDetailRepository transactionDetailsRepository,
        IEventTicketDetailRepository eventTicketDetailRepository,
        ITicketCategoryRepository ticketCategoryRepository,
        IEventDetailRepository eventDetailRepository,
        IEventAttributeRepository eventAttributeRepository,
        IEventTicketAttributeRepository eventTicketAttributeRepository,
        IEventRepository eventRepository)
        {
            _transactionRepository = transactionRepository;
            _transactionDetailsRepository = transactionDetailsRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _eventDetailRepository = eventDetailRepository;
            _eventAttributeRepository = eventAttributeRepository;
            _eventRepository = eventRepository;
        }

        public ICCPaymentCheckQueryResult Handle(ICCPaymentCheckQuery query)
        {
            List<FIL.Contracts.DataModels.Transaction> transactionByData = new List<Contracts.DataModels.Transaction>();
            FIL.Contracts.Models.EventDetail matchEventDetails = new Contracts.Models.EventDetail();
            bool isPurchase = false;
            TicketLimitCheckTypes ticketLimitErrorBy = TicketLimitCheckTypes.None;

            foreach (TicketLimitCheckTypes type in Enum.GetValues(typeof(TicketLimitCheckTypes)))
            {
                if (type == TicketLimitCheckTypes.Email)
                {
                    transactionByData = _transactionRepository.GetSuccessFullTransactionByEmail(query.UserEmail).ToList();
                }
                else if (type == TicketLimitCheckTypes.None)
                {
                    transactionByData = _transactionRepository.GetSuccessFullTransactionByPhone(query.UserPhone).ToList();
                }
                if (transactionByData.Count() > 0)
                {
                    foreach (FIL.Contracts.DataModels.Transaction transaction in transactionByData)
                    {
                        var transactionDetails = _transactionDetailsRepository.GetByTransactionId(transaction.Id);

                        var eventTicketAttributeDetails = _eventTicketAttributeRepository.GetByEventTicketAttributeIds(transactionDetails.Select(s => s.EventTicketAttributeId));

                        var eventTicketDetails = _eventTicketDetailRepository.GetByEventTicketDetailIds(eventTicketAttributeDetails.Select(s => s.EventTicketDetailId));

                        var eventDetails = _eventDetailRepository.GetByEventDetailIds(eventTicketDetails.Select(s => s.EventDetailId).Distinct());

                        foreach (FIL.Contracts.DataModels.EventDetail eventDetail in eventDetails)
                        {
                            var tEventTicketDetail = _eventTicketDetailRepository.GetByEventDetailIdsAndIds(eventDetails.Where(w => w.Id == eventDetail.Id).Select(s => s.Id), eventTicketDetails.Select(s => s.Id));

                            var tEventTicketAttribute = _eventTicketAttributeRepository.GetByEventTicketDetailIds(tEventTicketDetail.Select(s => s.Id)).Distinct();

                            var tTransactionDetail = _transactionDetailsRepository.GetByEventTicketAttributeandTransactionId(tEventTicketAttribute.Select(s => s.Id), transaction.Id).Distinct();

                            bool hasMatch = tEventTicketAttribute.Select(x => x.Id)
                                  .Intersect(query.eventTicketAttribute.Select(s => s.Id))
                                  .Any();

                            var transactionTotalTickets = tTransactionDetail.Select(s => s.TotalTickets).Sum(item => item);

                            var commanEventTicketAttributesSum = query.eventTicketAttribute.Where(b => tEventTicketAttribute.Any(a => a.Id == b.Id)).Sum(s => s.TotalTickets);

                            if ((commanEventTicketAttributesSum + transactionTotalTickets) > 10 && hasMatch)
                            {
                                if (type == TicketLimitCheckTypes.Email)
                                {
                                    ticketLimitErrorBy = TicketLimitCheckTypes.Email;
                                }
                                else
                                {
                                    ticketLimitErrorBy = TicketLimitCheckTypes.Phone;
                                }
                                matchEventDetails = AutoMapper.Mapper.Map<FIL.Contracts.Models.EventDetail>(eventDetail);
                                isPurchase = true;
                                break;
                            }
                        }
                        if (isPurchase)
                        {
                            break;
                        }
                    }
                }
            }
            return new ICCPaymentCheckQueryResult
            {
                IsTicketLimitExceed = isPurchase,
                eventDetails = matchEventDetails,
                TicketLimitErrorBy = ((ticketLimitErrorBy == TicketLimitCheckTypes.None) ? "" : ticketLimitErrorBy.ToString())
            };
        }
    }
}