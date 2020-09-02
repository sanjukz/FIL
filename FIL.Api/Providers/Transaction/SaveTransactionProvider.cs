using FIL.Api.Modules.SiteExtensions;
using FIL.Api.Providers.Zoom;
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
    public interface ISaveTransactionProvider
    {
        CheckoutCommandResult SaveTransaction(CheckoutCommand checkoutCommand,
            IEnumerable<Contracts.Models.EventTicketAttribute> eventTicketAttributeModel,
            FIL.Contracts.DataModels.User user);
    }

    public class SaveTransactionProvider : ISaveTransactionProvider
    {
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionDetailRepository _transactionDetailRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IReferralProvider _referralProvider;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly ISaveIPProvider _saveIPProvider;
        private readonly IGeoCurrency _geoCurrency;
        private readonly IEventRepository _eventRepository;
        private readonly ISeatBlockingProvider _seatBlockingProvider;
        private readonly IZoomMeetingProvider _zoomMeetingProvider;
        private readonly IEventStripeConnectAccountProvider _eventStripeConnectAccountProvider;
        private readonly ISaveTransactionScheduleDetailProvider _saveTransactionScheduleDetailProvider;
        private readonly FIL.Logging.ILogger _logger;

        public SaveTransactionProvider(ILogger logger, ISettings settings,
            IEventTicketAttributeRepository eventTicketAttributeRepository,
            ITransactionRepository transactionRepository,
            ITransactionDetailRepository transactionDetailRepository,
            ITicketCategoryRepository ticketCategoryRepository,
            ICountryRepository countryRepository,
            IReferralProvider referralProvider,
            ISaveTransactionScheduleDetailProvider saveTransactionScheduleDetailProvider,
            IEventDetailRepository eventDetailRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            IGeoCurrency geoCurrency,
            ISaveIPProvider saveIPProvider,
            ISeatBlockingProvider seatBlockingProvider,
            IZoomMeetingProvider zoomMeetingProvider,
            IEventRepository eventRepository,
            IEventStripeConnectAccountProvider eventStripeConnectAccountProvider
            )
        {
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _transactionRepository = transactionRepository;
            _transactionDetailRepository = transactionDetailRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _eventDetailRepository = eventDetailRepository;
            _referralProvider = referralProvider;
            _countryRepository = countryRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _saveIPProvider = saveIPProvider;
            _geoCurrency = geoCurrency;
            _zoomMeetingProvider = zoomMeetingProvider;
            _eventStripeConnectAccountProvider = eventStripeConnectAccountProvider;
            _eventRepository = eventRepository;
            _saveTransactionScheduleDetailProvider = saveTransactionScheduleDetailProvider;
            _logger = logger;
            _seatBlockingProvider = seatBlockingProvider;
        }

        public CheckoutCommandResult SaveTransaction(CheckoutCommand checkoutCommand, IEnumerable<Contracts.Models.EventTicketAttribute> eventTicketAttributeModel, FIL.Contracts.DataModels.User user)
        {
            try
            {
                var isPaymentBypass = false;
                var StripeAccount = FIL.Contracts.Enums.StripeAccount.None;
                List<TransactionDetail> transactionDetailList = new List<TransactionDetail>();
                List<FIL.Contracts.Models.CartItemModel> lstEventDetailId = new List<FIL.Contracts.Models.CartItemModel>();
                FIL.Contracts.DataModels.Transaction transaction = new FIL.Contracts.DataModels.Transaction();

                decimal grossTicketAmount = 0;
                long eventDetailId = 0, ticketCategoryId = 0;
                decimal netTicketAmount = 0;
                decimal totalDiscountAmount = 0;

                if (checkoutCommand.IsASI == null)
                {
                    checkoutCommand.IsASI = false;
                }

                var allETD = _eventTicketDetailRepository.GetByEventTicketDetailsIds(eventTicketAttributeModel.Select(s => s.EventTicketDetailId).Distinct()).Distinct();
                var allED = _eventDetailRepository.GetByIds(allETD.Select(s => s.EventDetailId).Distinct()).Distinct();
                foreach (Contracts.Commands.Transaction.EventTicketAttribute ticketAttributes in checkoutCommand.EventTicketAttributeList)
                {
                    var currentTA = ticketAttributes;
                    var transactionType = checkoutCommand.IsQrTransaction ? TransactionType.QRCode : checkoutCommand.TransactionType == TransactionType.Itinerary ? TransactionType.Itinerary : ticketAttributes.TicketType == TicketType.SeasonPackage ? TransactionType.Season : ticketAttributes.TransactionType == TransactionType.LiveOnline ? TransactionType.LiveOnline : ticketAttributes.TransactionType == TransactionType.AddOns ? TransactionType.AddOns : TransactionType.Regular;
                    decimal discountAmount = 0, donationAmount = 0;
                    if (ticketAttributes.DiscountedPrice > 0)
                    {
                        discountAmount = ticketAttributes.DiscountedPrice;
                    }
                    if (ticketAttributes.DonationAmount != null && ticketAttributes.DonationAmount > 0)
                    {
                        donationAmount = (decimal)ticketAttributes.DonationAmount;
                    }
                    Contracts.Models.EventTicketAttribute checkoutCommandEventTicketAttribute = eventTicketAttributeModel.Where(w => w.Id == ticketAttributes.Id).FirstOrDefault();
                    decimal pricePerTicket = checkoutCommandEventTicketAttribute.Price;
                    EventTicketDetail eventTicketDetail = allETD.Where(s => s.Id == checkoutCommandEventTicketAttribute.EventTicketDetailId && s.IsEnabled).FirstOrDefault();
                    if (eventTicketDetail != null)
                    {
                        EventDetail eventDetail = allED.Where(s => s.Id == eventTicketDetail.EventDetailId && s.IsEnabled).FirstOrDefault();
                        if (eventDetail != null)
                        {
                            var visitStartDate = ticketAttributes.VisitDate;
                            var visitEndDate = ticketAttributes.VisitDate;
                            if (checkoutCommand.TransactionType == TransactionType.Itinerary)
                            {
                                visitStartDate = ticketAttributes.VisitStartTime.Split(":").Count() > 1 ? new DateTime(visitStartDate.Year, visitStartDate.Month, visitStartDate.Day, Convert.ToInt32(ticketAttributes.VisitStartTime.Split(":")[0]), Convert.ToInt32(ticketAttributes.VisitStartTime.Split(":")[1]), 0) : visitStartDate;
                                visitEndDate = ticketAttributes.VisitEndTime.Split(":").Count() > 1 ? new DateTime(visitEndDate.Year, visitEndDate.Month, visitEndDate.Day, Convert.ToInt32(ticketAttributes.VisitEndTime.Split(":")[0]), Convert.ToInt32(ticketAttributes.VisitEndTime.Split(":")[1]), 0) : visitEndDate;
                            }
                            visitStartDate = visitStartDate < new DateTime(1753, 1, 1) ? DateTime.UtcNow : visitStartDate;
                            visitEndDate = visitEndDate < new DateTime(1753, 1, 1) ? DateTime.UtcNow : visitEndDate;
                            if ((bool)checkoutCommand.IsASI)
                            {
                                pricePerTicket = ticketAttributes.TicketType == TicketType.Child ? 0 : checkoutCommandEventTicketAttribute.Price;
                            }
                            if (ticketAttributes.OverridedAmount != null && checkoutCommand.IsBSPUpgrade && ticketAttributes.OverridedAmount != 0)
                            {
                                pricePerTicket = (decimal)ticketAttributes.OverridedAmount;
                            }
                            if (ticketAttributes.TicketType == TicketType.SeasonPackage)
                            {
                                eventDetailId = eventTicketDetail.EventDetailId;
                                lstEventDetailId.Add(new FIL.Contracts.Models.CartItemModel { EventDetailId = eventDetailId });
                                ticketCategoryId = eventTicketDetail.TicketCategoryId;
                                IEnumerable<EventDetail> seasonEventDetails = _eventDetailRepository.GetSeasonEventDetails(eventDetail.EventId, eventDetail.VenueId).Where(s => s.IsEnabled == true);
                                IEnumerable<EventTicketDetail> seasonEventTicketDetails = _eventTicketDetailRepository.GetByEventDetailIds(seasonEventDetails.Select(s => s.Id).Distinct()).Where(w => w.TicketCategoryId == eventTicketDetail.TicketCategoryId);
                                List<Contracts.DataModels.EventTicketAttribute> seasonEventTicketAttributes = _eventTicketAttributeRepository.GetByEventTicketDetailIds(seasonEventTicketDetails.Select(s => s.Id).Distinct()).Where(W => W.IsEnabled == true && W.SeasonPackage == true).ToList();
                                var seasonPrice = seasonEventTicketAttributes[0].SeasonPackagePrice;
                                pricePerTicket = seasonPrice / seasonEventTicketAttributes.Count;
                            }
                            else
                            {
                                eventDetailId = eventDetail.Id;
                                lstEventDetailId.Add(new FIL.Contracts.Models.CartItemModel { EventDetailId = eventTicketDetail.EventDetailId });
                                ticketCategoryId = eventTicketDetail.TicketCategoryId;
                            }

                            if (Convert.ToInt16(ticketAttributes.TotalTickets) <= checkoutCommandEventTicketAttribute.RemainingTicketForSale)
                            {
                                TransactionDetail transactionDetail = new TransactionDetail();
                                transactionDetail.EventTicketAttributeId = ticketAttributes.Id;
                                transactionDetail.TotalTickets = Convert.ToInt16(ticketAttributes.TotalTickets);
                                transactionDetail.PricePerTicket = checkoutCommand.TransactionType == TransactionType.Itinerary ? ticketAttributes.Price : pricePerTicket;
                                transactionDetail.DiscountAmount = discountAmount;
                                transactionDetail.VisitDate = visitStartDate;
                                transactionDetail.VisitEndDate = visitEndDate;
                                transactionDetail.TransactionType = transactionType;
                                transactionDetail.TicketTypeId = checkoutCommand.TransactionType == TransactionType.Itinerary ? (short)(ticketAttributes.IsAdult ? 10 : 2) : (short)(TicketType)ticketAttributes.TicketType;
                                if (checkoutCommand.ReferralId != null)
                                {
                                    var referral = _referralProvider.GetReferral(checkoutCommand.ReferralId, eventDetail.EventId, checkoutCommand.ModifiedBy);
                                    if (referral != null && referral.Id != 0)
                                    {
                                        transactionDetail.ReferralId = referral.Id;
                                    }
                                }
                                if (checkoutCommand.ChannelId == Channels.Feel && checkoutCommand.TransactionType != TransactionType.Itinerary)
                                {
                                    _geoCurrency.UpdateTransactionUpdates(transactionDetail, checkoutCommand.TargetCurrencyCode, checkoutCommandEventTicketAttribute.CurrencyId);
                                }
                                if (donationAmount > 0) // Donation doesn't need the local currency as the amount itself in the local currency
                                {
                                    transactionDetail.PricePerTicket = transactionDetail.PricePerTicket + donationAmount;
                                }
                                netTicketAmount += ((ticketAttributes.TotalTickets * (decimal)transactionDetail.PricePerTicket) - ((decimal)transactionDetail.DiscountAmount));
                                grossTicketAmount += ((ticketAttributes.TotalTickets * transactionDetail.PricePerTicket));
                                totalDiscountAmount += (decimal)transactionDetail.DiscountAmount;
                                transactionDetail.MembershipId = ticketAttributes.MembershipId;
                                transactionDetailList.Add(transactionDetail);
                            }
                            else
                            {
                                EventDetail eventDetails = _eventDetailRepository.Get(eventDetailId);
                                Contracts.DataModels.TicketCategory ticketCategory = _ticketCategoryRepository.Get((int)ticketCategoryId);
                                return new CheckoutCommandResult
                                {
                                    Id = 0,
                                    Success = false,
                                    EventName = eventDetails.Name,
                                    TicketCategoryName = ticketAttributes.TicketType == TicketType.SeasonPackage ? "Season - " + ticketCategory.Name : ticketCategory.Name,
                                    IsTransactionLimitExceed = false,
                                    IsTicketCategorySoldOut = true
                                };
                            }
                        }
                        else
                        {
                            EventDetail eventDetails = _eventDetailRepository.Get(eventDetailId);
                            Contracts.DataModels.TicketCategory ticketCategory = _ticketCategoryRepository.Get((int)ticketCategoryId);
                            return new CheckoutCommandResult
                            {
                                Id = 0,
                                Success = false,
                                EventName = eventDetails.Name,
                                TicketCategoryName = ticketAttributes.TicketType == TicketType.SeasonPackage ? "Season - " + ticketCategory.Name : ticketCategory.Name,
                                IsTransactionLimitExceed = false,
                                IsTicketCategorySoldOut = true
                            };
                        }
                    }
                    else
                    {
                        EventDetail eventDetails = _eventDetailRepository.Get(eventDetailId);
                        Contracts.DataModels.TicketCategory ticketCategory = _ticketCategoryRepository.Get((int)ticketCategoryId);
                        return new CheckoutCommandResult
                        {
                            Id = 0,
                            Success = false,
                            EventName = eventDetails.Name,
                            TicketCategoryName = ticketAttributes.TicketType == TicketType.SeasonPackage ? "Season - " + ticketCategory.Name : ticketCategory.Name,
                            IsTransactionLimitExceed = false,
                            IsTicketCategorySoldOut = true
                        };
                    }
                }
                var intialCurrencyId = eventTicketAttributeModel.Select(s => s.CurrencyId).FirstOrDefault();
                var currencyId = eventTicketAttributeModel.Select(s => s.CurrencyId).FirstOrDefault();
                if (checkoutCommand.ChannelId == Channels.Feel) // Update the currencyId
                {
                    if (checkoutCommand.TransactionType == TransactionType.Itinerary)
                    {
                        currencyId = _geoCurrency.GetCurrencyID(checkoutCommand.TransactionCurrency).Id;
                    }
                    else
                    {
                        currencyId = _geoCurrency.GetCurrencyID(checkoutCommand.TargetCurrencyCode).Id;
                    }
                }

                /*if (checkoutCommand.ISRasv) // NAB test bed
                {
                   var splitprice = netTicketAmount.ToString("0.00").Split(".");
                    var price = splitprice[0] + ".00";
                   netTicketAmount = Convert.ToDecimal(netTicketAmount);
                }*/

                transaction.ChannelId = checkoutCommand.ChannelId;
                transaction.CurrencyId = currencyId;
                transaction.TotalTickets = Convert.ToInt16(checkoutCommand.EventTicketAttributeList.Sum(s => s.TotalTickets));
                transaction.GrossTicketAmount = grossTicketAmount;
                transaction.NetTicketAmount = netTicketAmount;
                transaction.DiscountAmount = totalDiscountAmount;
                transaction.TransactionStatusId = TransactionStatus.UnderPayment;
                transaction.FirstName = user.FirstName;
                transaction.LastName = user.LastName;
                transaction.PhoneCode = user.PhoneCode;
                transaction.PhoneNumber = user.Email == "asi@zoonga.com" ? checkoutCommand.GuestUser.PhoneNumber : user.PhoneNumber;
                transaction.EmailId = user.Email == "asi@zoonga.com" ? checkoutCommand.GuestUser.PhoneNumber : user.Email;
                transaction.CountryName = !string.IsNullOrWhiteSpace(user.PhoneCode) ? _countryRepository.GetByPhoneCode(user.PhoneCode).Name : "India";
                transaction.CreatedBy = user.AltId;
                transaction.CreatedUtc = DateTime.UtcNow;

                try
                {
                    var ipDetail = _saveIPProvider.SaveIp(checkoutCommand.Ip);
                    if (ipDetail != null && ipDetail.Id > 0)
                    {
                        transaction.IPDetailId = ipDetail.Id;
                    }
                }
                catch (Exception e)
                {
                    _logger.Log(Logging.Enums.LogCategory.Error, e);
                    transaction.IPDetailId = 1;
                }

                transaction.ModifiedBy = user.AltId;
                transaction.AltId = Guid.NewGuid();
                if (transaction.CurrencyId != 7)
                {
                    transaction.OTP = checkoutCommand.OTPCode;
                }
                if (checkoutCommand.DonationAmount != null && checkoutCommand.DonationAmount != 0)
                {
                    transaction.DonationAmount = checkoutCommand.DonationAmount;
                }

                FIL.Contracts.DataModels.Transaction transactionResult = _transactionRepository.Save(transaction);
                foreach (TransactionDetail transactionDetail in transactionDetailList)
                {
                    transactionDetail.TransactionId = transactionResult.Id;
                    var currentTransactionDetail = _transactionDetailRepository.Save(transactionDetail);
                    FIL.Contracts.DataModels.EventTicketAttribute eventTicketAttribute = AutoMapper.Mapper.Map<FIL.Contracts.DataModels.EventTicketAttribute>(_eventTicketAttributeRepository.Get(transactionDetail.EventTicketAttributeId));
                    FIL.Contracts.DataModels.EventTicketDetail eventTicketDetail = AutoMapper.Mapper.Map<FIL.Contracts.DataModels.EventTicketDetail>(_eventTicketDetailRepository.Get(eventTicketAttribute.EventTicketDetailId));
                    if (Convert.ToInt16(transactionDetail.TotalTickets) <= eventTicketAttribute.RemainingTicketForSale)
                    {
                        eventTicketAttribute.RemainingTicketForSale -= transactionDetail.TotalTickets;
                        _eventTicketAttributeRepository.Save(eventTicketAttribute);
                        if (checkoutCommand.ChannelId == Channels.Website)
                        {
                            if (eventTicketDetail.InventoryTypeId == InventoryType.Seated || eventTicketDetail.InventoryTypeId == InventoryType.SeatedWithSeatSelection || eventTicketDetail.InventoryTypeId == InventoryType.NoneSeated)
                            {
                                List<Contracts.Models.TMS.SeatDetail> seatDetail = new List<Contracts.Models.TMS.SeatDetail>();
                                if (checkoutCommand.SeatDetails != null)
                                {
                                    seatDetail = AutoMapper.Mapper.Map<List<Contracts.Models.TMS.SeatDetail>>(checkoutCommand.SeatDetails.Where(w => w.EventTicketDetailId == eventTicketDetail.Id));
                                }
                                else
                                {
                                    seatDetail = null;
                                }
                                var seatBlock = _seatBlockingProvider.BlockSeat(seatDetail, transactionDetail, eventTicketAttribute, eventTicketDetail, user.AltId, Channels.Website);
                                if (!seatBlock.Success && seatBlock.IsSeatSoldOut)
                                {
                                    return new CheckoutCommandResult
                                    {
                                        Success = false,
                                        Id = 0,
                                        TransactionAltId = Guid.NewGuid(),
                                        IsTransactionLimitExceed = false,
                                        IsTicketCategorySoldOut = false,
                                        IsSeatSoldOut = true
                                    };
                                }
                            }
                        }
                    }
                    else
                    {
                        EventDetail eventDetails = _eventDetailRepository.Get(eventDetailId);
                        Contracts.DataModels.TicketCategory ticketCategory = _ticketCategoryRepository.Get((int)ticketCategoryId);
                        return new CheckoutCommandResult
                        {
                            Id = 0,
                            Success = false,
                            EventName = eventDetails.Name,
                            TicketCategoryName = transactionDetail.TicketTypeId == (short)TicketType.SeasonPackage ? "Season - " + ticketCategory.Name : ticketCategory.Name,
                            IsTransactionLimitExceed = false,
                            IsTicketCategorySoldOut = true
                        };
                    }
                    /* Save Transaction Schedule Detail */
                    var currentTicket = checkoutCommand.EventTicketAttributeList.Where(s => s.Id == currentTransactionDetail.EventTicketAttributeId).FirstOrDefault();
                    if (currentTicket.ScheduleDetailId != null && currentTicket.ScheduleDetailId != 0)
                    {
                        _saveTransactionScheduleDetailProvider.SaveTransactionScheduleDetail(currentTransactionDetail.Id, (long)currentTicket.ScheduleDetailId);
                    }
                }

                try
                {
                    if (checkoutCommand.ChannelId == Channels.Feel || checkoutCommand.ChannelId == Channels.Website) // If Live Online Transaction with 0.00 price
                    {
                        var @event = _eventRepository.Get(allED.FirstOrDefault().EventId);
                        if (@event != null)
                        {
                            if (@event.MasterEventTypeId == Contracts.Enums.MasterEventType.Online && transaction.NetTicketAmount == 0)
                            {
                                _zoomMeetingProvider.CreateMeeting(transaction, true);
                                isPaymentBypass = true;
                            }
                            else if (@event.MasterEventTypeId == Contracts.Enums.MasterEventType.Online)
                            {
                                StripeAccount = _eventStripeConnectAccountProvider.GetEventStripeAccount(allED.FirstOrDefault().EventId, checkoutCommand.ChannelId);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.Log(Logging.Enums.LogCategory.Error, e);
                }
                try
                {
                    /*Check if referred transactionId already upgraded to BSP or successfull transaction, if yes then don't allow another transaction using same referral/ transaction id*/
                    if (checkoutCommand.IsBSPUpgrade && checkoutCommand.ReferralId != null)
                    {
                        var transactions = _transactionRepository.GetAllSuccessfulTransactionByReferralId(checkoutCommand.ReferralId);
                        if (transactions.Any())
                        {
                            return new CheckoutCommandResult
                            {
                                Id = 0,
                                Success = false,
                                IsBSPUpgraded = true
                            };
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.Log(Logging.Enums.LogCategory.Error, e);
                }

                return new CheckoutCommandResult
                {
                    Id = transactionResult.Id,
                    CartBookingModel = lstEventDetailId,
                    Transaction = transactionResult,
                    IsPaymentByPass = isPaymentBypass,
                    Success = true,
                    StripeAccount = StripeAccount,
                };
            }
            catch (Exception e)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, e);
                return new CheckoutCommandResult
                {
                };
            }
        }
    }
}