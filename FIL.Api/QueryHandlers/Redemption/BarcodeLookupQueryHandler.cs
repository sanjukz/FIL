using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.Redemption;
using FIL.Contracts.QueryResults.Redemption;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace FIL.Api.QueryHandlers.Redemption
{
    public class BarcodeLookupQueryHandler : IQueryHandler<BarcodeLookupQuery, BarcodeLookupQueryResult>
    {
        private readonly IMatchSeatTicketDetailRepository _matchSeatTicketDetailRepository;
        private readonly ITransactionDetailRepository _transactionDetailRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;
        private readonly IUserAddressDetailRepository _userAddressDetailRepository;
        private readonly IZipcodeRepository _zipcodeRepository;
        private readonly IUserRepository _userRepository;
        private readonly FIL.Logging.ILogger _logger;

        public BarcodeLookupQueryHandler(IMatchSeatTicketDetailRepository matchSeatTicketDetailRepository, ITransactionDetailRepository transactionDetailRepository, ITransactionRepository transactionRepository, IEventTicketAttributeRepository eventTicketAttributeRepository, IEventTicketDetailRepository eventTicketDetailRepository, ITicketCategoryRepository ticketCategoryRepository, ICurrencyTypeRepository currencyTypeRepository, IUserAddressDetailRepository userAddressDetailRepository, IZipcodeRepository zipcodeRepository, IUserRepository userRepository, FIL.Logging.ILogger logger)
        {
            _matchSeatTicketDetailRepository = matchSeatTicketDetailRepository;
            _transactionDetailRepository = transactionDetailRepository;
            _transactionRepository = transactionRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _currencyTypeRepository = currencyTypeRepository;
            _userAddressDetailRepository = userAddressDetailRepository;
            _zipcodeRepository = zipcodeRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public BarcodeLookupQueryResult Handle(BarcodeLookupQuery query)
        {
            try
            {
                var eventTicketDetails = _eventTicketDetailRepository.GetRASVRideEventTicketDetails()
                    .ToDictionary(k => k.Id);
                var matchSeatTicketDetails = _matchSeatTicketDetailRepository.GetByTransactionIdAndTicketDetailIds(query.TransactionId, eventTicketDetails.Keys)
                    .ToList();
                if (matchSeatTicketDetails.Any())
                {
                    var transaction = _transactionRepository.Get((long)matchSeatTicketDetails.First().TransactionId);
                    var currency = _currencyTypeRepository.GetById(transaction.CurrencyId);
                    var userAltId = transaction.CreatedBy;
                    var userId = _userRepository.GetByAltId(userAltId).Id;
                    var userAddressDetail = _userAddressDetailRepository.GetByUser(userId);
                    var zipcodeId = userAddressDetail != null ? userAddressDetail.Zipcode : 9;
                    var postalcode = _zipcodeRepository.Get(zipcodeId).Postalcode;
                    // var ticketCategories = _ticketCategoryRepository.GetByEventDetailIds(eventTicketDetails.Keys).ToDictionary(k =>k.Id);
                    var ticketCategories = _ticketCategoryRepository.GetByTicketCategoryIds(eventTicketDetails.Select(s => s.Value.TicketCategoryId)).ToDictionary(k => k.Id);
                    var eventTicketAttributes = _eventTicketAttributeRepository.GetByEventTicketDetailIds(eventTicketDetails.Keys).ToDictionary(k => k.EventTicketDetailId);

                    var barcodedetails = matchSeatTicketDetails.Select(mst =>
                    {
                        var eventTicketDetail = eventTicketDetails[mst.EventTicketDetailId];
                        var attributes = eventTicketAttributes[eventTicketDetail.Id];
                        var value = JsonConvert.DeserializeObject<TicketValue>(attributes.AdditionalInfo).value;

                        return new BarcodeDetailsContainer
                        {
                            AltId = (System.Guid)mst.AltId,
                            BarcodeNumber = mst.BarcodeNumber,
                            CurrencyCode = currency.Code,
                            TicketCategoryId = eventTicketDetail.TicketCategoryId,
                            TicketCategory = ticketCategories[(int)eventTicketDetail.TicketCategoryId].Name,
                            Value = value,
                            Postalcode = postalcode,
                            IsConsumed = mst.IsConsumed,
                            ConsumedDateTime = mst.ConsumedDateTime,
                            TransactionUTC = transaction.CreatedUtc,
                        };
                    });

                    return new BarcodeLookupQueryResult
                    {
                        BarcodeDetailsContainer = barcodedetails.ToList(),
                        IsValid = true
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
            }

            return new BarcodeLookupQueryResult
            {
                BarcodeDetailsContainer = null,
                IsValid = false
            };
        }
    }
}