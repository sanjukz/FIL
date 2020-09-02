using FIL.Api.Modules.SiteExtensions;
using FIL.Api.Providers.Zoom;
using FIL.Api.Repositories;
using FIL.Contracts.Commands.Discount;
using FIL.Contracts.DataModels;
using FIL.Contracts.Interfaces.Commands;
using FIL.Logging;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Discount
{
    public class DiscountCommandHandler : BaseCommandHandlerWithResult<ApplyDiscountCommand, ApplyDiscountCommandResult>
    {
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IIPDetailRepository _iPDetailRepository;
        private readonly ITransactionDetailRepository _transactionDetailRepository;
        private readonly IEventTicketDiscountDetailRepository _eventTicketDiscountDetailRepository;
        private readonly IDiscountPromoCodeRepository _discountPromoCodeRepository;
        private readonly IZoomMeetingProvider _zoomMeetingProvider;
        private readonly IDiscountRepository _discountRepository;
        private readonly IEventCategoryRepository _eventCategoryRepository;
        private readonly FIL.Logging.ILogger _logger;
        private readonly IGeoCurrency _geoCurrency;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;

        public DiscountCommandHandler(
            IEventTicketAttributeRepository eventTicketAttributeRepository, ITransactionRepository transactionRepository,
            ITransactionDetailRepository transactionDetailRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            IEventDetailRepository eventDetailRepository,
            IEventRepository eventRepository,
            IVenueRepository venueRepository,
            ICityRepository cityRepository,
            IStateRepository stateRepository,
            ICountryRepository countryRepository,
            IIPDetailRepository iPDetailRepository,
            IEventTicketDiscountDetailRepository eventTicketDiscountDetailRepository,
            IDiscountRepository discountRepository,
            IDiscountPromoCodeRepository discountPromoCodeRepository,
            IZoomMeetingProvider zoomMeetingProvider,
            IEventCategoryRepository eventCategoryRepository,
            ILogger logger,
            IGeoCurrency geoCurrency,
            ICurrencyTypeRepository currencyTypeRepository,
            IMediator mediator) : base(mediator)
        {
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _transactionRepository = transactionRepository;
            _transactionDetailRepository = transactionDetailRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventDetailRepository = eventDetailRepository;
            _venueRepository = venueRepository;
            _cityRepository = cityRepository;
            _stateRepository = stateRepository;
            _eventRepository = eventRepository;
            _countryRepository = countryRepository;
            _iPDetailRepository = iPDetailRepository;
            _eventTicketDiscountDetailRepository = eventTicketDiscountDetailRepository;
            _discountPromoCodeRepository = discountPromoCodeRepository;
            _discountRepository = discountRepository;
            _eventCategoryRepository = eventCategoryRepository;
            _zoomMeetingProvider = zoomMeetingProvider;
            _geoCurrency = geoCurrency;
            _currencyTypeRepository = currencyTypeRepository;
            _logger = logger;
        }

        protected override Task<ICommandResult> Handle(ApplyDiscountCommand command)
        {
            var transaction = _transactionRepository.Get(command.TransactionId);

            var transactionDetails = _transactionDetailRepository.GetByTransactionId(command.TransactionId);
            var ipDetail = _iPDetailRepository.Get((int)transaction.IPDetailId);
            decimal totalDiscountAmount = 0;
            decimal discountAmount = 0;

            foreach (TransactionDetail currentTransactionDetail in transactionDetails)
            {
                var eventTicketDetailDiscount = _eventTicketDiscountDetailRepository.GetAllByEventTicketAttributeId(currentTransactionDetail.EventTicketAttributeId).ToList();
                if (eventTicketDetailDiscount.Count() > 0 && currentTransactionDetail.TicketTypeId != 4)
                {
                    List<long> discountIds = new List<long>();
                    foreach (var currentEventTicketDetailDiscount in eventTicketDetailDiscount)
                    {
                        discountIds.Add(currentEventTicketDetailDiscount.DiscountId);
                    }
                    var discounts = _discountRepository.GetAllDiscountsByIds(discountIds).Where(w => w.IsEnabled);
                    if (discounts.Count() > 0)
                    {
                        List<long> Ids = new List<long>();
                        foreach (var id in discounts)
                        {
                            Ids.Add(id.Id);
                        }
                        var discountPromoCode = _discountPromoCodeRepository.GetAllDiscountIds(Ids).Where(s => s.PromoCode == command.Promocode).FirstOrDefault();
                        if (discountPromoCode != null)
                        {
                            //Check for limit
                            bool limitFlag = true;
                            if (discountPromoCode.Limit != null && discountPromoCode.Limit != 0)
                            {
                                var promoCodeTransactions = _transactionRepository.GetSuccessfullTransactionByPromoCode(discountPromoCode.PromoCode).ToList();
                                var promocodeTransactionDetails = _transactionDetailRepository.GetByTransactionIds(promoCodeTransactions.Select(s => s.Id)).Where(w => w.EventTicketAttributeId == currentTransactionDetail.EventTicketAttributeId);

                                if (promocodeTransactionDetails.Count() >= discountPromoCode.Limit)
                                {
                                    limitFlag = false;
                                }
                            }
                            if (limitFlag)
                            {
                                if (discounts.Where(s => s.Id == discountPromoCode.DiscountId).FirstOrDefault().DiscountValueTypeId == Contracts.Enums.DiscountValueType.Flat)
                                {
                                    discountAmount = (decimal)(currentTransactionDetail.TotalTickets * discounts.Where(s => s.Id == discountPromoCode.DiscountId).FirstOrDefault().DiscountValue);
                                    if (discountAmount == (currentTransactionDetail.PricePerTicket * currentTransactionDetail.TotalTickets))
                                    {
                                        currentTransactionDetail.ConvenienceCharges = 0;
                                        currentTransactionDetail.ServiceCharge = 0;
                                    }
                                }
                                else if (discounts.Where(s => s.Id == discountPromoCode.DiscountId).FirstOrDefault().DiscountValueTypeId == Contracts.Enums.DiscountValueType.Percentage)
                                {
                                    discountAmount = (decimal)(((currentTransactionDetail.PricePerTicket * currentTransactionDetail.TotalTickets) * (decimal)discounts.Where(s => s.Id == discountPromoCode.DiscountId).FirstOrDefault().DiscountValue) / 100);
                                    if (discountAmount == (currentTransactionDetail.PricePerTicket * currentTransactionDetail.TotalTickets))
                                    {
                                        currentTransactionDetail.ConvenienceCharges = 0;
                                        currentTransactionDetail.ServiceCharge = 0;
                                    }
                                }
                                totalDiscountAmount += discountAmount;

                                if (command.Channel == Contracts.Enums.Channels.Feel)
                                {
                                    var eventTicketAttribute = _eventTicketAttributeRepository.Get(currentTransactionDetail.EventTicketAttributeId);
                                    var eventCurreny = _currencyTypeRepository.Get(eventTicketAttribute.CurrencyId);
                                    var targetCurrency = _currencyTypeRepository.Get(transaction.CurrencyId);
                                    if (discounts.Where(s => s.Id == discountPromoCode.DiscountId).FirstOrDefault().DiscountValueTypeId == Contracts.Enums.DiscountValueType.Flat)
                                    {
                                        discountAmount = _geoCurrency.GetConvertedDiscountAmount((Decimal)discountAmount, eventCurreny.Id, targetCurrency.Code);
                                        totalDiscountAmount = _geoCurrency.GetConvertedDiscountAmount((Decimal)totalDiscountAmount, eventCurreny.Id, targetCurrency.Code);
                                    }
                                }
                                currentTransactionDetail.DiscountAmount = discountAmount;
                                _transactionDetailRepository.Save(currentTransactionDetail);
                            }
                        }
                    }
                }
            }

            ApplyDiscountCommandResult applyDiscountCommandResult = new ApplyDiscountCommandResult();

            if (totalDiscountAmount == 0)
            {
                applyDiscountCommandResult.Id = 0;
            }
            else
            {
                transaction.DiscountAmount = totalDiscountAmount;
                transaction.NetTicketAmount = transaction.NetTicketAmount - totalDiscountAmount;
                if (transaction.GrossTicketAmount == totalDiscountAmount)
                {
                    transaction.NetTicketAmount = 0;
                    transaction.ConvenienceCharges = 0;
                    transaction.ServiceCharge = 0;
                }
                transaction.DiscountCode = command.Promocode;
                _transactionRepository.Save(transaction);

                applyDiscountCommandResult.Id = transaction.Id;
                applyDiscountCommandResult.CurrencyId = transaction.CurrencyId;
                applyDiscountCommandResult.GrossTicketAmount = transaction.GrossTicketAmount;
                applyDiscountCommandResult.DeliveryCharges = transaction.DeliveryCharges;
                applyDiscountCommandResult.ConvenienceCharges = transaction.ConvenienceCharges;
                applyDiscountCommandResult.ServiceCharge = transaction.ServiceCharge;
                applyDiscountCommandResult.DiscountAmount = transaction.DiscountAmount;
                applyDiscountCommandResult.NetTicketAmount = transaction.NetTicketAmount;
            }

            try
            {
                if (transaction.NetTicketAmount <= 0)
                {
                    var liveOnlineTransactionDetailModel = _transactionRepository.GetFeelOnlineDetails(transaction.Id).FirstOrDefault();
                    if (liveOnlineTransactionDetailModel != null)
                    {
                        //check if subcategory is LiveOnline
                        var eventCategoryModel = _eventCategoryRepository.Get(Convert.ToInt16(liveOnlineTransactionDetailModel.EventcategoryId));
                        var eventModel = _eventRepository.Get(liveOnlineTransactionDetailModel.EventId);
                        if ((eventCategoryModel != null && eventModel.MasterEventTypeId == Contracts.Enums.MasterEventType.Online && command.Channel == Contracts.Enums.Channels.Feel) || eventModel.EventCategoryId == 119)
                        {
                            _zoomMeetingProvider.CreateMeeting(transaction, true);
                            applyDiscountCommandResult.IsPaymentBypass = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, e);
            }

            return Task.FromResult<ICommandResult>(applyDiscountCommandResult);
        }
    }
}