using FIL.Api.PaymentChargers;
using FIL.Api.Repositories;
using FIL.Contracts.Commands.Payment;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using FIL.Contracts.Models.PaymentChargers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Payment
{
    public class PaymentCommandHandler : BaseCommandHandlerWithResult<PaymentCommand, PaymentCommandResult>
    {
        private readonly IHdfcChargerResolver _hdfcChargerResolver;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;
        private readonly IPaymentHtmlPostCharger<IHdfcEnrolledCharge, IPaymentHtmlPostResponse> _hdfcPaymentHtmlPostCharger;
        private readonly IPaymentCharger<IHdfcCharge, IPaymentResponse> _hdfcCharger;
        private readonly IUserRepository _userRepository;
        private readonly IUserCardDetailRepository _userCardDetailRepository;
        private readonly IPaymentHtmlPostCharger<ICcavenueCharge, IPaymentHtmlPostResponse> _ccavenuePaymentHtmlPostCharger;
        private readonly IPaymentCharger<IStripeCharge, IPaymentResponse> _stripeCharger;
        private readonly IPaymentHtmlPostCharger<INabTransactCharge, IPaymentHtmlPostResponse> _nabTransactCharger;
        private readonly IIPDetailRepository _ipDetailRepository;
        private readonly IUserAddressDetailRepository _userAddressDetailRepository;
        private readonly IZipcodeRepository _zipcodeRepository;
        private readonly ICityRepository _cityRepository;
        private readonly FIL.Logging.ILogger _logger;
        private readonly IMatchSeatTicketDetailRepository _matchSeatTicketDetailRepository;
        private readonly ITransactionSeatDetailRepository _transactionSeatDetailRepository;
        private readonly ITransactionDetailRepository _transactionDetailRepository;
        private readonly IMatchLayoutSectionSeatRepository _matchLayoutSectionSeatRepository;

        public PaymentCommandHandler(IHdfcChargerResolver hdfcChargerResolver,
            ITransactionRepository transactionRepository,
            ICurrencyTypeRepository currencyTypeRepository,
            IPaymentHtmlPostCharger<IHdfcEnrolledCharge, IPaymentHtmlPostResponse> hdfcPaymentHtmlPostCharger,
            IPaymentCharger<IHdfcCharge, IPaymentResponse> hdfcCharger,
            IPaymentHtmlPostCharger<ICcavenueCharge, IPaymentHtmlPostResponse> ccavenuePaymentHtmlPostCharger,
            IUserRepository userRepository,
            IUserCardDetailRepository userCardDetailRepository,
            IPaymentCharger<IStripeCharge, IPaymentResponse> stripeCharger,
            IPaymentHtmlPostCharger<INabTransactCharge, IPaymentHtmlPostResponse> nabTransactCharger,
            IIPDetailRepository ipDetailRepository, IUserAddressDetailRepository userAddressDetailRepository,
            IZipcodeRepository zipcodeRepository,
            ICityRepository cityRepository,
            FIL.Logging.ILogger logger,
            IMediator mediator,
            IMatchSeatTicketDetailRepository matchSeatTicketDetailRepository,
            ITransactionSeatDetailRepository transactionSeatDetailRepository,
            ITransactionDetailRepository transactionDetailRepository,
            IMatchLayoutSectionSeatRepository matchLayoutSectionSeatRepository
            )
            : base(mediator)
        {
            _hdfcChargerResolver = hdfcChargerResolver;
            _transactionRepository = transactionRepository;
            _currencyTypeRepository = currencyTypeRepository;
            _hdfcPaymentHtmlPostCharger = hdfcPaymentHtmlPostCharger;
            _hdfcCharger = hdfcCharger;
            _ccavenuePaymentHtmlPostCharger = ccavenuePaymentHtmlPostCharger;
            _userRepository = userRepository;
            _userAddressDetailRepository = userAddressDetailRepository;
            _userRepository = userRepository;
            _zipcodeRepository = zipcodeRepository;
            _userCardDetailRepository = userCardDetailRepository;
            _stripeCharger = stripeCharger;
            _nabTransactCharger = nabTransactCharger;
            _ipDetailRepository = ipDetailRepository;
            _cityRepository = cityRepository;
            _logger = logger;
            _matchSeatTicketDetailRepository = matchSeatTicketDetailRepository;
            _transactionSeatDetailRepository = transactionSeatDetailRepository;
            _transactionDetailRepository = transactionDetailRepository;
            _matchLayoutSectionSeatRepository = matchLayoutSectionSeatRepository;
        }

        protected override async Task<ICommandResult> Handle(PaymentCommand query)
        {
            var transaction = _transactionRepository.Get(query.TransactionId);
            var currency = _currencyTypeRepository.Get(transaction.CurrencyId);
            User user = _userRepository.GetByEmail(transaction.EmailId);

            UserCardDetail userCardDetail = _userCardDetailRepository.GetByUserCardNumber(query.PaymentCard.CardNumber, user.Id);
            if (userCardDetail == null)
            {
                UserCardDetail obj = new UserCardDetail
                {
                    UserId = user.Id,
                    AltId = new Guid(),
                    NameOnCard = query.PaymentCard.NameOnCard ?? string.Empty,
                    CardNumber = query.PaymentCard.CardNumber ?? string.Empty,
                    ExpiryMonth = query.PaymentCard.ExpiryMonth,
                    ExpiryYear = query.PaymentCard.ExpiryYear,
                    CardTypeId = query.PaymentCard.CardType
                };
                userCardDetail = _userCardDetailRepository.Save(obj);
            }

            try
            {
                if (query.BillingAddress != null)
                {
                    if (query.BillingAddress.Zipcode == null)
                    {
                        query.BillingAddress.Zipcode = "110016";
                    }

                    var zipcode = _zipcodeRepository.GetByZipcode(query.BillingAddress.Zipcode.ToString());
                    if (zipcode == null)
                    {
                        var city = _cityRepository.GetByName(query.BillingAddress.City);
                        var zipCode = new Zipcode
                        {
                            AltId = Guid.NewGuid(),
                            Postalcode = query.BillingAddress.Zipcode.ToString(),
                            CityId = city != null ? city.Id : 0,
                            IsEnabled = true
                        };

                        _zipcodeRepository.Save(zipCode);

                        zipcode = _zipcodeRepository.GetByZipcode(query.BillingAddress.Zipcode.ToString());
                    }
                    if (user != null && zipcode != null)
                    {
                        var addressDetail = new UserAddressDetail
                        {
                            UserId = user.Id,
                            AltId = Guid.NewGuid(),
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            PhoneCode = user.PhoneCode,
                            PhoneNumber = user.PhoneNumber,
                            AddressLine1 = query.BillingAddress.Address,
                            Zipcode = zipcode.Id,
                            AddressTypeId = AddressTypes.Billing,
                            IsEnabled = true
                        };

                        _userAddressDetailRepository.Save(addressDetail);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
            }

            if (query.PaymentGateway != null)
            {
                List<FIL.Contracts.DataModels.TransactionDetail> transactionDetails = _transactionDetailRepository.GetByTransactionId(transaction.Id).ToList();
                if (transactionDetails.Any())
                {
                    List<FIL.Contracts.DataModels.TransactionSeatDetail> transactionSeatDetails = _transactionSeatDetailRepository.GetByTransactionDetailIds(transactionDetails.Select(s => s.Id)).ToList();
                    if (transactionSeatDetails != null)
                    {
                        var matchSeatTicketDetail = _matchSeatTicketDetailRepository.GetByMatchSeatTicketDetailIds(transactionSeatDetails.Select(s => s.MatchSeatTicketDetailId)).ToList();

                        if (matchSeatTicketDetail != null && matchSeatTicketDetail.Any())
                        {
                            return new PaymentCommandResult
                            {
                                TransactionAltId = transaction.AltId,
                                PaymentResponse = new PaymentResponse
                                {
                                    Success = false,
                                    PaymentGatewayError = PaymentGatewayError.Unknown
                                }
                            };
                        }
                    }
                }
            }

            if (query.PaymentGateway == PaymentGateway.Stripe)
            {
                bool isIntentConfirm = false;
                if (query.PaymentCard.NameOnCard != null)
                    isIntentConfirm = query.PaymentCard.NameOnCard.Equals("intent", StringComparison.InvariantCultureIgnoreCase);
                var paymentResponse = await _stripeCharger.Charge(new StripeCharge
                {
                    TransactionId = transaction.Id,
                    Currency = currency.Code,
                    Amount = Convert.ToDecimal(transaction.NetTicketAmount),
                    UserCardDetailId = userCardDetail.Id,
                    Token = query.Token,
                    ChannelId = query.ChannelId,
                    IsIntentConfirm = isIntentConfirm
                });
                return new PaymentCommandResult
                {
                    TransactionAltId = transaction.AltId,
                    PaymentResponse = paymentResponse
                };
            }

            if (query.PaymentGateway != null && query.PaymentGateway == PaymentGateway.NabTransact)
            {
                var paymentHtmlPostResponse = await _nabTransactCharger.Charge(new NabTransactCharge
                {
                    TransactionId = transaction.Id,
                    Currency = currency.Code,
                    Amount = Convert.ToDecimal(transaction.NetTicketAmount),
                    UserCardDetailId = userCardDetail.Id,
                    PaymentCard = new FIL.Contracts.Models.PaymentChargers.PaymentCard
                    {
                        CardNumber = query.PaymentCard.CardNumber,
                        NameOnCard = query.PaymentCard.NameOnCard,
                        Cvv = query.PaymentCard.Cvv,
                        ExpiryMonth = query.PaymentCard.ExpiryMonth,
                        ExpiryYear = query.PaymentCard.ExpiryYear,
                        CardType = query.PaymentCard.CardType
                    },
                    BillingAddress = new FIL.Contracts.Models.PaymentChargers.BillingAddress
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PhoneCode = user.PhoneCode,
                        PhoneNumber = user.PhoneNumber,
                        Email = user.Email,
                        AddressLine1 = !string.IsNullOrWhiteSpace(query.BillingAddress.Address) ? query.BillingAddress.Address : "Zoonga",
                        Zipcode = !string.IsNullOrWhiteSpace(query.BillingAddress.Zipcode.ToString()) ? query.BillingAddress.Zipcode.ToString() : "3032",
                        City = !string.IsNullOrWhiteSpace(query.BillingAddress.City) ? query.BillingAddress.City : "Delhi",
                        State = !string.IsNullOrWhiteSpace(query.BillingAddress.State) ? query.BillingAddress.State : "Delhi",
                        Country = !string.IsNullOrWhiteSpace(query.BillingAddress.Country) ? query.BillingAddress.Country : "India"
                    },
                    PaymentOption = PaymentOptions.CreditCard,
                    User = new FIL.Contracts.Models.User
                    {
                        Email = user.Email
                    },
                    //IPAddress = ipDetail.IPAddress
                });

                return new PaymentCommandResult
                {
                    TransactionAltId = transaction.AltId,
                    PaymentHtmlPostResponse = paymentHtmlPostResponse
                };
            }

            if (PaymentOptions.CreditCard == query.PaymentOption || PaymentOptions.DebitCard == query.PaymentOption)
            {
                IHdfcEnrollmentResponse hdfcEnrollmentResponse = _hdfcChargerResolver.HdfcEnrollmentVerification(new HdfcCharge
                {
                    TransactionId = transaction.Id,
                    Currency = currency.Code,
                    Amount = Convert.ToDecimal(transaction.NetTicketAmount),
                    UserCardDetailId = userCardDetail.Id,
                    PaymentCard = new FIL.Contracts.Models.PaymentChargers.PaymentCard
                    {
                        CardNumber = query.PaymentCard.CardNumber,
                        NameOnCard = query.PaymentCard.NameOnCard,
                        Cvv = query.PaymentCard.Cvv,
                        ExpiryMonth = query.PaymentCard.ExpiryMonth,
                        ExpiryYear = query.PaymentCard.ExpiryYear,
                        CardType = query.PaymentCard.CardType
                    }
                });

                if (hdfcEnrollmentResponse.PaymentGatewayError == PaymentGatewayError.None)
                {
                    if (hdfcEnrollmentResponse.HdfcEnrolledCharge.Result.ToString() == "ENROLLED")
                    {
                        var paymentHtmlPostResponse = await _hdfcPaymentHtmlPostCharger.Charge(hdfcEnrollmentResponse.HdfcEnrolledCharge);
                        return new PaymentCommandResult
                        {
                            TransactionAltId = transaction.AltId,
                            PaymentHtmlPostResponse = paymentHtmlPostResponse
                        };
                    }

                    if (hdfcEnrollmentResponse.HdfcEnrolledCharge.Result.ToString() == "NOT ENROLLED")
                    {
                        var paymentResponse = await _hdfcCharger.Charge(new HdfcCharge
                        {
                            TransactionId = transaction.Id,
                            Currency = currency.Code,
                            Amount = Convert.ToDecimal(transaction.NetTicketAmount),
                            UserCardDetailId = userCardDetail.Id,
                            PaymentCard = new FIL.Contracts.Models.PaymentChargers.PaymentCard
                            {
                                CardNumber = query.PaymentCard.CardNumber,
                                NameOnCard = query.PaymentCard.NameOnCard,
                                Cvv = query.PaymentCard.Cvv,
                                ExpiryMonth = query.PaymentCard.ExpiryMonth,
                                ExpiryYear = query.PaymentCard.ExpiryYear,
                                CardType = query.PaymentCard.CardType
                            }
                        });

                        return new PaymentCommandResult
                        {
                            TransactionAltId = transaction.AltId,
                            PaymentResponse = paymentResponse
                        };
                    }
                }

                return new PaymentCommandResult
                {
                    TransactionAltId = transaction.AltId,
                    PaymentResponse = new PaymentResponse
                    {
                        Success = false,
                        PaymentGatewayError = hdfcEnrollmentResponse.PaymentGatewayError
                    }
                };
            }

            if (PaymentOptions.NetBanking == query.PaymentOption || PaymentOptions.CashCard == query.PaymentOption)
            {
                var paymentHtmlPostResponse = await _ccavenuePaymentHtmlPostCharger.Charge(new CcavenueCharge
                {
                    TransactionId = transaction.Id,
                    Currency = currency.Code,
                    Amount = Convert.ToDecimal(transaction.NetTicketAmount),
                    UserCardDetailId = userCardDetail.Id,
                    PaymentCard = new FIL.Contracts.Models.PaymentChargers.PaymentCard
                    {
                        CardNumber = query.PaymentCard.CardNumber,
                        NameOnCard = query.PaymentCard.NameOnCard,
                        Cvv = query.PaymentCard.Cvv,
                        ExpiryMonth = query.PaymentCard.ExpiryMonth,
                        ExpiryYear = query.PaymentCard.ExpiryYear,
                        CardType = query.PaymentCard.CardType
                    },
                    PaymentOption = (PaymentOptions)query.PaymentOption,
                    BillingAddress = new FIL.Contracts.Models.PaymentChargers.BillingAddress
                    {
                        FirstName = !string.IsNullOrWhiteSpace(transaction.FirstName) ? transaction.FirstName : "Zoonga",
                        LastName = !string.IsNullOrWhiteSpace(transaction.LastName) ? transaction.LastName : "Zoonga",
                        PhoneCode = !string.IsNullOrWhiteSpace(transaction.PhoneCode) ? transaction.PhoneCode : "91",
                        PhoneNumber = !string.IsNullOrWhiteSpace(transaction.PhoneNumber) ? transaction.PhoneNumber : "9899704772",
                        Email = !string.IsNullOrWhiteSpace(transaction.EmailId) ? transaction.EmailId : "accounts@zoonga.com",
                        //FirstName = "Gaurav",
                        //LastName = "Bhardwaj",
                        //PhoneCode = "91",
                        //PhoneNumber = "9899704772",
                        //Email = "gaurav.bhardwaj@zoonga.com",
                        AddressLine1 = !string.IsNullOrWhiteSpace(query.BillingAddress.Address) ? query.BillingAddress.Address : "Zoonga",
                        Zipcode = !string.IsNullOrWhiteSpace(query.BillingAddress.Zipcode.ToString()) ? query.BillingAddress.Zipcode.ToString() : "110016",
                        City = !string.IsNullOrWhiteSpace(query.BillingAddress.City) ? query.BillingAddress.City : "Delhi",
                        State = !string.IsNullOrWhiteSpace(query.BillingAddress.State) ? query.BillingAddress.State : "Delhi",
                        Country = !string.IsNullOrWhiteSpace(query.BillingAddress.Country) ? query.BillingAddress.Country : "India"
                    },
                    BankAltId = query.BankAltId,
                    CardAltId = query.CardAltId,
                    ChannelId = query.ChannelId,
                });

                return new PaymentCommandResult
                {
                    TransactionAltId = transaction.AltId,
                    PaymentHtmlPostResponse = paymentHtmlPostResponse
                };
            }

            return new PaymentCommandResult
            {
                TransactionAltId = transaction.AltId,
                PaymentResponse = new PaymentResponse
                {
                    Success = false,
                    PaymentGatewayError = PaymentGatewayError.Unknown
                }
            };
        }
    }
}