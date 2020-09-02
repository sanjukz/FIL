using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.BoxOffice;
using FIL.Contracts.QueryResults.BoxOffice;
using System;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.BoxOffice
{
    public class GetTicketDataQueryHandler : IQueryHandler<GetTicketDataQuery, GetTicketDataQueryResult>
    {
        private readonly IMatchSeatTicketDetailRepository _matchSeatTicketDetailRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionDetailRepository _transactionDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly ITicketRefundDetailRepository _ticketRefundDetailRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;
        private readonly IBoCustomerDetailRepository _boCustomerDetailRepository;

        public GetTicketDataQueryHandler(IMatchSeatTicketDetailRepository matchSeatTicketDetailRepository, ITransactionRepository transactionRepository, ITransactionDetailRepository transactionDetailRepository, IEventTicketAttributeRepository eventTicketAttributeRepository, IEventTicketDetailRepository eventTicketDetailRepository, IEventDetailRepository eventDetailRepository, ITicketCategoryRepository ticketCategoryRepository, ITicketRefundDetailRepository ticketRefundDetailRepository, ICurrencyTypeRepository currencyTypeRepository, IBoCustomerDetailRepository boCustomerDetailRepository)
        {
            _matchSeatTicketDetailRepository = matchSeatTicketDetailRepository;
            _transactionRepository = transactionRepository;
            _transactionDetailRepository = transactionDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventDetailRepository = eventDetailRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _ticketRefundDetailRepository = ticketRefundDetailRepository;
            _currencyTypeRepository = currencyTypeRepository;
            _boCustomerDetailRepository = boCustomerDetailRepository;
        }

        public GetTicketDataQueryResult Handle(GetTicketDataQuery query)
        {
            var matchSeatticketDetails = _matchSeatTicketDetailRepository.GetByBarcodeNumber(query.BarcodeNumber);
            if (matchSeatticketDetails == null)
            {
                return new GetTicketDataQueryResult
                {
                    IsValid = false,
                    errorMessage = "Invalid barcode number"
                };
            }
            else
            {
                var isSuccessTransction = new FIL.Contracts.DataModels.Transaction();
                if (query.IsRefund)
                {
                    isSuccessTransction = _transactionRepository.GetSuccessfulBOTransactionDetails(Convert.ToInt64(matchSeatticketDetails.TransactionId));
                }
                else
                {
                    isSuccessTransction = _transactionRepository.GetSuccessfulTransactionDetails(Convert.ToInt64(matchSeatticketDetails.TransactionId));
                }
                if (isSuccessTransction == null)
                {
                    return new GetTicketDataQueryResult
                    {
                        IsValid = false,
                        errorMessage = "Invalid barcode number"
                    };
                }
                else
                {
                    var barcodeDetails = _ticketRefundDetailRepository.GetByBarcodeNumber(query.BarcodeNumber);
                    if (barcodeDetails == null)
                    {
                        var eventTicketAttribute = _eventTicketAttributeRepository.GetByEventTicketDetailId(matchSeatticketDetails.EventTicketDetailId);
                        var transactionDetails = _transactionDetailRepository.GetByEventTicketAttributeIdAndTransactionIdAndTicketTypeId(Convert.ToInt64(matchSeatticketDetails.TransactionId), eventTicketAttribute.Id, Convert.ToInt16(matchSeatticketDetails.TicketTypeId));
                        var eventTicketDetail = _eventTicketDetailRepository.Get(matchSeatticketDetails.EventTicketDetailId);
                        var eventDetails = _eventDetailRepository.Get(eventTicketDetail.EventDetailId);
                        var ticketCategory = _ticketCategoryRepository.Get((int)eventTicketDetail.TicketCategoryId);
                        var currencyType = _currencyTypeRepository.GetAll();
                        var boxOfficeUserData = _boCustomerDetailRepository.GetAllByTransactionId(Convert.ToInt64(matchSeatticketDetails.TransactionId));
                        return new GetTicketDataQueryResult
                        {
                            IsValid = true,
                            Transaction = AutoMapper.Mapper.Map<FIL.Contracts.Models.Transaction>(isSuccessTransction),
                            TransactionDetail = AutoMapper.Mapper.Map<TransactionDetail>(transactionDetails),
                            MatchSeatTicketDetail = AutoMapper.Mapper.Map<MatchSeatTicketDetail>(matchSeatticketDetails),
                            EventDetail = AutoMapper.Mapper.Map<EventDetail>(eventDetails),
                            TicketCategory = AutoMapper.Mapper.Map<FIL.Contracts.Models.TicketCategory>(ticketCategory),
                            CurrencyType = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.CurrencyType>>(currencyType),
                            BoCustomerDetails = AutoMapper.Mapper.Map<List<FIL.Contracts.DataModels.BoCustomerDetail>>(boxOfficeUserData),
                        };
                    }
                    else
                    {
                        return new GetTicketDataQueryResult
                        {
                            IsValid = false,
                            errorMessage = "Barcode number already refunded!"
                        };
                    }
                }
            }
        }
    }
}