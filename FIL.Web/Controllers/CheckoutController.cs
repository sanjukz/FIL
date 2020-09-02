using FIL.Contracts.Commands.CitySightSeeing;
using FIL.Contracts.Commands.Tiqets;
using FIL.Contracts.Commands.Transaction;
using FIL.Contracts.Enums;
using FIL.Contracts.Queries.CurrentOrderData;
using FIL.Contracts.Queries.User;
using FIL.Foundation.Senders;
using FIL.Logging.Enums;
using FIL.MailChimp;
using FIL.Messaging.Senders;
using FIL.Web.Core.Helpers;
using FIL.Web.Core.Providers;
using FIL.Web.Core.UrlsProvider;
using FIL.Web.Feel.Modules.SiteExtensions;
using FIL.Web.Feel.ViewModels.Checkout;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IQuerySender _querySender;
        private readonly IPasswordHasher<string> _passwordHasher;
        private readonly IAuthenticationHelper _authenticationHelper;
        private readonly ISessionProvider _sessionProvider;
        private readonly IClientIpProvider _clientIpProvider;
        private readonly ISiteIdProvider _siteIdProvider;
        private readonly ISiteUrlsProvider _siteUrlsProvider;
        private readonly IConfirmationEmailSender _confirmationEmailSender;
        private readonly IGeoCurrency _geoCurrency;
        private readonly FIL.Logging.ILogger _logger;
        private readonly IMailChimpProvider _mailChimpProvider;

        public CheckoutController(ICommandSender commandSender,
            IQuerySender querySender,
            IPasswordHasher<string> passwordHasher,
            IAuthenticationHelper authenticationHelper,
            ISessionProvider sessionProvider,
            IConfirmationEmailSender confirmationEmailSender,
            IClientIpProvider clientIpProvider,
            ISiteIdProvider siteIdProvider,
            ISiteUrlsProvider siteUrlsProvider,
            IGeoCurrency geoCurrency,
            FIL.Logging.ILogger logger,
            IMailChimpProvider mailChimpProvider)
        {
            _commandSender = commandSender;
            _querySender = querySender;
            _passwordHasher = passwordHasher;
            _authenticationHelper = authenticationHelper;
            _sessionProvider = sessionProvider;
            _confirmationEmailSender = confirmationEmailSender;
            _clientIpProvider = clientIpProvider;
            _siteIdProvider = siteIdProvider;
            _siteUrlsProvider = siteUrlsProvider;
            _logger = logger;
            _geoCurrency = geoCurrency;
            _mailChimpProvider = mailChimpProvider;
        }

        [HttpPost]
        [Route("api/loginuser/transaction")]
        public async Task<UserCheckoutResponseViewModel> Login([FromBody] LoginTransactionFormDataViewModel model)
        {
            model.UserDetail.ChannelId = Channels.Feel;
            var authenticated = await _authenticationHelper.AuthenticateUser(model.UserDetail, u =>
            {
                return Task.FromResult(true);
            });

            if (authenticated)
            {
                var queryResult = await _querySender.Send(new LoginUserQuery
                {
                    Email = model.UserDetail.Email,
                    Password = model.UserDetail.Password,
                    ChannelId = Channels.Feel,
                });
                var clientIpAddress = _clientIpProvider.Get();
                //reserve timeslot for Hoho if any
                ReserveTimeSlotCommandResult resereBookingResult = new ReserveTimeSlotCommandResult();
                var IsTimeSlotBooking = model.EventTicketAttributeList.Where(s => s.ReserveHohoBook == true).ToList();
                if (IsTimeSlotBooking.Count() > 0)
                {
                    resereBookingResult = await _commandSender.Send<ReserveTimeSlotCommand, ReserveTimeSlotCommandResult>(new ReserveTimeSlotCommand
                    {
                        EventTicketAttributeList = model.EventTicketAttributeList.Where(s => s.ReserveHohoBook == true).ToList(),
                        UserAltId = queryResult.User.AltId,
                    });
                    if (!resereBookingResult.Success)
                    {
                        return new UserCheckoutResponseViewModel
                        {
                            Success = false,
                            Session = await _sessionProvider.Get(),
                            TransactionId = 0,
                        };
                    }
                }

                CreateOrderCommandResult createOrderResult = new CreateOrderCommandResult();
                // Check if place is Tiqets one
                if (model.IsTiqets)
                {
                    createOrderResult = await _commandSender.Send<CreateOrderCommand, CreateOrderCommandResult>(new CreateOrderCommand
                    {
                        EventTicketAttributeList = model.EventTicketAttributeList,
                        UserAltId = queryResult.User.AltId,
                    });
                }
                if ((model.IsTiqets && createOrderResult != null && createOrderResult.Success) || !model.IsTiqets)
                {
                    CheckoutCommandResult transactionResult = await _commandSender.Send<CheckoutCommand, CheckoutCommandResult>(new CheckoutCommand
                    {
                        ChannelId = Contracts.Enums.Channels.Feel,
                        EventTicketAttributeList = model.EventTicketAttributeList,
                        UserAltId = queryResult.User.AltId,
                        Ip = clientIpAddress,
                        TransactionType = model.IsItinerary ? TransactionType.Itinerary : TransactionType.Regular,
                        TransactionCurrency = model.TransactionCurrency,
                        TargetCurrencyCode = _geoCurrency.GetSessionCurrency(HttpContext),
                        IsLoginCheckout = true,
                        ReferralId = model.ReferralId,
                        DonationAmount = model.DonationAmount
                    });
                    if (model.IsTiqets)
                    {
                        await _commandSender.Send(new Contracts.Commands.Tiqets.SaveOrderCommand
                        {
                            TransactionId = transactionResult.Id,
                            PaymentToken = createOrderResult.PaymentToken,
                            OrderRefernceId = createOrderResult.OrderRefernceId,
                            ModifiedBy = queryResult.User.AltId
                        });
                    }
                    if (IsTimeSlotBooking.Count() > 0 && resereBookingResult.Success)
                    {
                        await _commandSender.Send(new Contracts.Commands.CitySightSeeing.SaveOrderCommand
                        {
                            TransactionId = transactionResult.Id,
                            FromTime = resereBookingResult.FromTime,
                            EndTime = resereBookingResult.EndTime,
                            Distributor_reference = resereBookingResult.Distributor_reference,
                            Reservation_reference = resereBookingResult.Reservation_reference,
                            Reservation_valid_until = resereBookingResult.Reservation_valid_until,
                            TicketId = resereBookingResult.TicketId,
                            TimeSlot = resereBookingResult.TimeSlot,
                            ModifiedBy = queryResult.User.AltId
                        });
                    }
                    var filteredEventVenueMapping = model.EventTicketAttributeList.Where(w => (w.EventVenueMappingTimeId != -1 || w.EventVenueMappingTimeId != 0));
                    foreach (var item in filteredEventVenueMapping.Where(s => s.EventVenueMappingTimeId != 0))
                    {
                        await _commandSender.Send(new TransactionMoveAroundMappingCommand
                        {
                            TransactionId = transactionResult.Id,
                            EventVenueMappingTimeId = item.EventVenueMappingTimeId,
                            PurchaserAddress = item.PurchaserAddress,
                            CreatedUtc = DateTime.UtcNow,
                            CreatedBy = Guid.NewGuid(),
                            ModifiedBy = Guid.NewGuid()
                        });
                    }

                    return new UserCheckoutResponseViewModel
                    {
                        IsPaymentByPass = transactionResult.IsPaymentByPass,
                        StripeAccount = transactionResult.StripeAccount,
                        Success = authenticated,
                        TransactionAltId = transactionResult.TransactionAltId,
                        Session = await _sessionProvider.Get(),
                        TransactionId = transactionResult.Id,
                    };
                }
                else
                {
                    return new UserCheckoutResponseViewModel
                    {
                        Success = false,
                        Session = await _sessionProvider.Get(),
                        TransactionId = 0,
                        IsTiqetsOrderFailure = true
                    };
                }

            }
            else
            {
                return new UserCheckoutResponseViewModel
                {
                    Success = false,
                    Session = await _sessionProvider.Get(),
                    TransactionId = 0,
                };
            }

        }

        [HttpPost]
        [Route("api/loginUserToDeliveryOption/transaction")]
        public async Task<UserCheckoutResponseViewModel> LoginUserToDeliveryOption([FromBody] GuestSignInToDeliveryTransactionFormDataViewModel model)
        {
            try
            {
                var clientIpAddress = _clientIpProvider.Get();
                //reserve timeslot for Hoho if any
                ReserveTimeSlotCommandResult resereBookingResult = new ReserveTimeSlotCommandResult();
                var IsTimeSlotBooking = model.EventTicketAttributeList.Where(s => s.ReserveHohoBook == true).ToList();
                if (IsTimeSlotBooking.Count() > 0)
                {
                    resereBookingResult = await _commandSender.Send<ReserveTimeSlotCommand, ReserveTimeSlotCommandResult>(new ReserveTimeSlotCommand
                    {
                        EventTicketAttributeList = model.EventTicketAttributeList.Where(s => s.ReserveHohoBook == true).ToList(),
                        UserAltId = model.UserAltId
                    });
                    if (!resereBookingResult.Success)
                    {
                        return new UserCheckoutResponseViewModel
                        {
                            Success = false,
                            Session = await _sessionProvider.Get(),
                            TransactionId = 0,
                        };
                    }
                }
                CreateOrderCommandResult createOrderResult = new CreateOrderCommandResult();
                // Check if place is Tiqets one
                if (model.IsTiqets)
                {
                    createOrderResult = await _commandSender.Send<CreateOrderCommand, CreateOrderCommandResult>(new CreateOrderCommand
                    {
                        EventTicketAttributeList = model.EventTicketAttributeList,
                        UserAltId = model.UserAltId
                    });
                }
                if ((model.IsTiqets && createOrderResult != null && createOrderResult.Success) || !model.IsTiqets)
                {
                    CheckoutCommandResult transactionResult = await _commandSender.Send<CheckoutCommand, CheckoutCommandResult>(new CheckoutCommand
                    {
                        ChannelId = Contracts.Enums.Channels.Feel,
                        EventTicketAttributeList = model.EventTicketAttributeList,
                        UserAltId = model.UserAltId,
                        Ip = clientIpAddress,
                        TransactionType = model.IsItinerary ? TransactionType.Itinerary : TransactionType.Regular,
                        TransactionCurrency = model.TransactionCurrency,
                        TargetCurrencyCode = _geoCurrency.GetSessionCurrency(HttpContext),
                        IsLoginCheckout = true,
                        ReferralId = model.ReferralId,
                        DonationAmount = model.DonationAmount,
                        IsBSPUpgrade = model.IsBSPUpgrade
                    });
                    if (model.IsTiqets)
                    {
                        await _commandSender.Send(new Contracts.Commands.Tiqets.SaveOrderCommand
                        {
                            TransactionId = transactionResult.Id,
                            PaymentToken = createOrderResult.PaymentToken,
                            OrderRefernceId = createOrderResult.OrderRefernceId,
                            ModifiedBy = model.UserAltId
                        });
                    }
                    if (IsTimeSlotBooking.Count() > 0 && resereBookingResult.Success)
                    {
                        await _commandSender.Send(new Contracts.Commands.CitySightSeeing.SaveOrderCommand
                        {
                            TransactionId = transactionResult.Id,
                            FromTime = resereBookingResult.FromTime,
                            EndTime = resereBookingResult.EndTime,
                            Distributor_reference = resereBookingResult.Distributor_reference,
                            Reservation_reference = resereBookingResult.Reservation_reference,
                            Reservation_valid_until = resereBookingResult.Reservation_valid_until,
                            TicketId = resereBookingResult.TicketId,
                            ModifiedBy = model.UserAltId
                        });
                    }

                    var filteredEventVenueMapping = model.EventTicketAttributeList.Where(w => (w.EventVenueMappingTimeId != -1 || w.EventVenueMappingTimeId != 0));

                    foreach (var item in filteredEventVenueMapping.Where(s => s.EventVenueMappingTimeId != 0))
                    {
                        await _commandSender.Send(new TransactionMoveAroundMappingCommand
                        {
                            TransactionId = transactionResult.Id,
                            EventVenueMappingTimeId = item.EventVenueMappingTimeId,
                            PurchaserAddress = item.PurchaserAddress,
                            CreatedUtc = DateTime.UtcNow,
                            CreatedBy = Guid.NewGuid(),
                            ModifiedBy = Guid.NewGuid()
                        });
                    }

                    try
                    {
                        var orderData = await _querySender.Send(new CurrentOrderDataQuery
                        {
                            TransactionAltId = transactionResult.TransactionAltId
                        });
                        orderData.Transaction.NetTicketAmount = _geoCurrency.Exchange((decimal)orderData.Transaction.NetTicketAmount, orderData.CurrencyType.Code);
                        orderData.Transaction.GrossTicketAmount = _geoCurrency.Exchange((decimal)orderData.Transaction.GrossTicketAmount, orderData.CurrencyType.Code);
                        orderData.Transaction.ServiceCharge = _geoCurrency.Exchange(orderData.Transaction.ServiceCharge != null ? (decimal)orderData.Transaction.ServiceCharge : 0, orderData.CurrencyType.Code);
                        orderData.Transaction.ConvenienceCharges = _geoCurrency.Exchange(orderData.Transaction.ConvenienceCharges != null ? (decimal)orderData.Transaction.ConvenienceCharges : 0, orderData.CurrencyType.Code);
                        foreach (var item in orderData.orderConfirmationSubContainer)
                        {
                            foreach (var eventContainer in item.subEventContainer)
                            {
                                foreach (var eta in eventContainer.EventTicketAttribute)
                                {
                                    _geoCurrency.eventTicketAttributeUpdate(eta, HttpContext, "USD");
                                }
                                foreach (var td in eventContainer.TransactionDetail)
                                {
                                    _geoCurrency.updateTransactionDetail(td, HttpContext, orderData.CurrencyType.Id, "USD");
                                }
                            }
                        }
                        await _mailChimpProvider.AddBuyerCart(orderData);
                    }
                    catch (Exception e)
                    {
                        _logger.Log(LogCategory.Error, e);
                    }

                    return new UserCheckoutResponseViewModel
                    {
                        Success = true,
                        IsPaymentByPass = transactionResult.IsPaymentByPass,
                        StripeAccount = transactionResult.StripeAccount,
                        TransactionAltId = transactionResult.TransactionAltId,
                        Session = await _sessionProvider.Get(),
                        TransactionId = transactionResult.Id,
                    };

                }
                else
                {
                    return new UserCheckoutResponseViewModel
                    {
                        Success = false,
                        Session = await _sessionProvider.Get(),
                        TransactionId = 0,
                        IsTiqetsOrderFailure = true
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, ex);
                return new UserCheckoutResponseViewModel
                {
                    Success = false,
                    Session = await _sessionProvider.Get(),
                    TransactionId = 0,
                };
            }
        }
    }
}