using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.Redemption;
using FIL.Contracts.QueryResults.Redemption;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace FIL.Api.QueryHandlers.Redemption
{
    public class BarcodeQueryHandler : IQueryHandler<BarcodeQuery, BarcodeQueryResult>
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
        private readonly IOfflineBarcodeDetailRepository _offlineBarcodeDetailRepository;
        private readonly FIL.Logging.ILogger _logger;

        public BarcodeQueryHandler(IMatchSeatTicketDetailRepository matchSeatTicketDetailRepository, ITransactionDetailRepository transactionDetailRepository, ITransactionRepository transactionRepository, IEventTicketAttributeRepository eventTicketAttributeRepository, IEventTicketDetailRepository eventTicketDetailRepository, ITicketCategoryRepository ticketCategoryRepository, ICurrencyTypeRepository currencyTypeRepository, IUserAddressDetailRepository userAddressDetailRepository, IZipcodeRepository zipcodeRepository, IUserRepository userRepository, IOfflineBarcodeDetailRepository offlineBarcodeDetailRepository, FIL.Logging.ILogger logger)
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
            _offlineBarcodeDetailRepository = offlineBarcodeDetailRepository;
            _logger = logger;
        }

        public BarcodeQueryResult Handle(BarcodeQuery query)
        {
            try
            {
                var eventTicketDetails = _eventTicketDetailRepository.GetRASVRideEventTicketDetails()
                    .ToDictionary(k => k.Id, k => k);

                var matchSeatTicketDetail = _matchSeatTicketDetailRepository.GetByBarcodeNumberAndEventTicketDetailIds(query.BarcodeNumber, eventTicketDetails.Keys);
                if (matchSeatTicketDetail != null)
                {
                    var transaction = _transactionRepository.Get((long)matchSeatTicketDetail.TransactionId);
                    var eventTicketDetail = eventTicketDetails[matchSeatTicketDetail.EventTicketDetailId];
                    var additionalInfo = _eventTicketAttributeRepository.GetByEventTicketDetailId(eventTicketDetail.Id).AdditionalInfo;
                    var value = JsonConvert.DeserializeObject<TicketValue>(additionalInfo).value;
                    var ticketCategoryId = eventTicketDetail.TicketCategoryId;
                    var ticketCategoryName = _ticketCategoryRepository.Get((int)ticketCategoryId).Name.ToString();
                    var currencyId = transaction.CurrencyId;
                    var transactionUTC = transaction.CreatedUtc;
                    var currencyCode = _currencyTypeRepository.GetById(currencyId).Code.ToString();
                    var userAltId = transaction.CreatedBy;
                    var userId = _userRepository.GetByAltId(userAltId).Id;
                    var userAddressDetail = _userAddressDetailRepository.GetByUser(userId);
                    var zipcodeId = userAddressDetail != null ? userAddressDetail.Zipcode : 9;
                    var postalcode = _zipcodeRepository.Get(zipcodeId).Postalcode;

                    var barcodeDetailsContainer = new BarcodeDetailsContainer()
                    {
                        AltId = (System.Guid)matchSeatTicketDetail.AltId,
                        BarcodeNumber = matchSeatTicketDetail.BarcodeNumber,
                        CurrencyCode = currencyCode,
                        TicketCategoryId = ticketCategoryId,
                        TicketCategory = ticketCategoryName,
                        Value = value,
                        Postalcode = postalcode,
                        IsConsumed = matchSeatTicketDetail.IsConsumed,
                        ConsumedDateTime = matchSeatTicketDetail.ConsumedDateTime,
                        TransactionUTC = transactionUTC,
                    };
                    return new BarcodeQueryResult
                    {
                        BarcodeDetailsContainer = barcodeDetailsContainer,
                        IsValid = true
                    };
                }

                var offlineBarcodeDetail = _offlineBarcodeDetailRepository.GetByBarcodeNumberAndEventTicketDetailIds(query.BarcodeNumber, eventTicketDetails.Keys);
                if (offlineBarcodeDetail != null)
                {
                    var ticketCategoryId = _eventTicketDetailRepository.Get(offlineBarcodeDetail.EventTicketDetailId)
                        .TicketCategoryId;
                    var ticketCategoryName = _ticketCategoryRepository.Get((int)ticketCategoryId).Name;
                    var currencyCode = _currencyTypeRepository.GetById(offlineBarcodeDetail.CurrencyId).Code;

                    var barcodeDetailsContainer = new BarcodeDetailsContainer
                    {
                        AltId = offlineBarcodeDetail.AltId,
                        BarcodeNumber = offlineBarcodeDetail.BarcodeNumber,
                        CurrencyCode = currencyCode,
                        TicketCategoryId = ticketCategoryId,
                        TicketCategory = ticketCategoryName,
                        Value = offlineBarcodeDetail.Price,
                        Postalcode = "",
                        IsConsumed = offlineBarcodeDetail.IsConsumed,
                        ConsumedDateTime = offlineBarcodeDetail.ConsumedDateTime,
                    };
                    return new BarcodeQueryResult
                    {
                        BarcodeDetailsContainer = barcodeDetailsContainer,
                        IsValid = true
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
            }

            return new BarcodeQueryResult
            {
                BarcodeDetailsContainer = null,
                IsValid = false
            };
        }
    }
}