using FIL.Api.Providers;
using FIL.Api.Providers.Transaction;
using FIL.Api.Providers.Zoom;
using FIL.Api.Repositories;
using FIL.Api.Repositories.Redemption;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.DataModels;
using FIL.Contracts.DataModels.Redemption;
using FIL.Contracts.Enums;
using FIL.Contracts.Models.PaymentChargers;
using FIL.Logging;
using FIL.Logging.Enums;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.PaymentChargers
{
    public interface IStripeCharger<IStripeCharge, IPaymentResponse> : IPaymentCharger<IStripeCharge, IPaymentResponse>
    {
    }

    public class StripeCharger : PaymentCharger<IStripeCharge>, IStripeCharger<IStripeCharge, IPaymentResponse>
    {
        private readonly ITransactionDetailRepository _transactionDetailRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionPaymentDetailRepository _transactionPaymentDetailRepository;
        private readonly ITransactionStatusUpdater _transactionStatusUpdater;
        private readonly IStripeConnectCharger _stripeConnectCharger;
        private readonly IEventRepository _eventRepository;
        private readonly IOrderDetailsRepository _orderDetailsRepository;
        private readonly IZoomMeetingProvider _zoomMeetingProvider;
        private readonly IEventStripeConnectAccountProvider _eventStripeConnectAccountProvider;

        public StripeCharger(ILogger logger,
            ISettings settings,
            ITransactionDetailRepository transactionDetailRepository,
            ITransactionRepository transactionRepository,
            ITransactionPaymentDetailRepository transactionPaymentDetailRepository,
            ITransactionStatusUpdater transactionStatusUpdater,
            IStripeConnectCharger stripeConnectCharger,
            IEventRepository eventRepository,
            IOrderDetailsRepository orderDetailsRepository,
            IEventStripeConnectAccountProvider eventStripeConnectAccountProvider,
            IZoomMeetingProvider zoomMeetingProvider)
            : base(logger, settings)
        {
            _transactionRepository = transactionRepository;
            _transactionDetailRepository = transactionDetailRepository;
            _transactionPaymentDetailRepository = transactionPaymentDetailRepository;
            _transactionStatusUpdater = transactionStatusUpdater;
            _stripeConnectCharger = stripeConnectCharger;
            _orderDetailsRepository = orderDetailsRepository;
            _eventRepository = eventRepository;
            _zoomMeetingProvider = zoomMeetingProvider;
            _eventStripeConnectAccountProvider = eventStripeConnectAccountProvider;
        }

        protected override async Task<IPaymentResponse> CreateCharge(IStripeCharge stripeChargeParameters)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(stripeChargeParameters.Token))
                {
                    if (stripeChargeParameters.IsIntentConfirm || stripeChargeParameters.Token.Contains("~"))
                    {
                        var splitToken = stripeChargeParameters.Token.Split('~');
                        string paymentMethodId = splitToken[1];
                        string paymentIntentId = splitToken[0];

                        var service = new PaymentIntentService();
                        var confirmOptions = new PaymentIntentConfirmOptions();

                        PaymentIntent paymentIntent = service.Confirm(paymentIntentId, confirmOptions, new RequestOptions
                        {
                            ApiKey = GetStripeToken(stripeChargeParameters)
                        });
                        if (paymentIntent.Status == "succeeded")
                        {
                            if (paymentIntent.Status == "succeeded")
                            {
                                Transaction transactionResult = _transactionStatusUpdater.UpdateTranscationStatus(Convert.ToInt64(stripeChargeParameters.TransactionId));
                                if (stripeChargeParameters.ChannelId == Channels.Feel)
                                {
                                    try
                                    {
                                        var redemptionOrder = new Redemption_OrderDetails
                                        {
                                            AltId = Guid.NewGuid(),
                                            ApprovedBy = null,
                                            ApprovedUtc = null,
                                            CreatedBy = transactionResult.CreatedBy,
                                            CreatedUtc = DateTime.UtcNow,
                                            IsEnabled = false,
                                            OrderCompletedDate = null,
                                            OrderStatusId = (int)FIL.Contracts.Enums.ApproveStatus.Pending,
                                            TransactionId = Convert.ToInt64(stripeChargeParameters.TransactionId),
                                            UpdatedBy = null,
                                            UpdatedUtc = null
                                        };
                                        _orderDetailsRepository.Save(redemptionOrder);
                                        _zoomMeetingProvider.CreateMeeting(transactionResult);
                                    }
                                    catch (Exception e)
                                    {
                                        _logger.Log(LogCategory.Error, new Exception("Failed to save the redemption order", e));
                                    }
                                }
                                else if (stripeChargeParameters.ChannelId == Channels.Website)
                                {
                                    _zoomMeetingProvider.CreateMeeting(transactionResult);
                                }
                            }

                            _transactionPaymentDetailRepository.Save(new TransactionPaymentDetail
                            {
                                TransactionId = Convert.ToInt64(stripeChargeParameters.TransactionId),
                                PaymentOptionId = PaymentOptions.CreditCard,
                                PaymentGatewayId = PaymentGateway.Stripe,
                                RequestType = "Charge Recieved",
                                Amount = stripeChargeParameters.Amount.ToString(),
                                PayConfNumber = paymentIntent.Id,
                                PaymentDetail = "{\"Response\":" + Newtonsoft.Json.JsonConvert.SerializeObject(paymentIntent) + "}",
                            });
                            return GetPaymentResponse(true, PaymentGatewayError.None);
                        }
                        else if ((paymentIntent.Status == "requires_source_action" || paymentIntent.Status == "requires_action") && paymentIntent.NextAction.Type == "use_stripe_sdk")
                        {
                            return GetPaymentResponse(false, PaymentGatewayError.RequireSourceAction, PaymentGateway.Stripe, paymentIntent.ClientSecret);
                        }
                        return GetPaymentResponse(false, GetPaymentGatewayErrorCode(paymentIntent.CancellationReason));
                    }

                    Stripe.PaymentIntent stripeCharge = CreateStripeCharge(stripeChargeParameters);
                    if (stripeCharge != null)
                    {
                        var paymentConfirmationNumber = stripeCharge.Id;
                        if ((stripeCharge.Status == "requires_source_action" || stripeCharge.Status == "requires_action") && stripeCharge.NextAction.Type == "use_stripe_sdk")
                        {
                            return GetPaymentResponse(false, PaymentGatewayError.RequireSourceAction, PaymentGateway.Stripe, stripeCharge.ClientSecret);
                        }

                        if (stripeCharge.Status == "succeeded")
                        {
                            Transaction transactionResult = _transactionStatusUpdater.UpdateTranscationStatus(Convert.ToInt64(stripeChargeParameters.TransactionId));
                            if (stripeChargeParameters.ChannelId == Channels.Feel)
                            {
                                try
                                {
                                    var redemptionOrder = new Redemption_OrderDetails
                                    {
                                        AltId = Guid.NewGuid(),
                                        ApprovedBy = null,
                                        ApprovedUtc = null,
                                        CreatedBy = transactionResult.CreatedBy,
                                        CreatedUtc = DateTime.UtcNow,
                                        IsEnabled = false,
                                        OrderCompletedDate = null,
                                        OrderStatusId = (int)FIL.Contracts.Enums.ApproveStatus.Pending,
                                        TransactionId = Convert.ToInt64(stripeChargeParameters.TransactionId),
                                        UpdatedBy = null,
                                        UpdatedUtc = null
                                    };
                                    _orderDetailsRepository.Save(redemptionOrder);
                                    _zoomMeetingProvider.CreateMeeting(transactionResult);
                                }
                                catch (Exception e)
                                {
                                    _logger.Log(LogCategory.Error, new Exception("Failed to save the redemption order", e));
                                }
                            }
                            else if (stripeChargeParameters.ChannelId == Channels.Website)
                            {
                                _zoomMeetingProvider.CreateMeeting(transactionResult);
                            }
                        }

                        _transactionPaymentDetailRepository.Save(new TransactionPaymentDetail
                        {
                            TransactionId = Convert.ToInt64(stripeChargeParameters.TransactionId),
                            PaymentOptionId = PaymentOptions.CreditCard,
                            PaymentGatewayId = PaymentGateway.Stripe,
                            RequestType = "Charge Recieved",
                            Amount = stripeChargeParameters.Amount.ToString(),
                            PayConfNumber = stripeCharge.Id,
                            PaymentDetail = "{\"Response\":" + Newtonsoft.Json.JsonConvert.SerializeObject(stripeCharge) + "}",
                        });

                        return stripeCharge.Status == "succeeded" ? GetPaymentResponse(true, PaymentGatewayError.None) : GetPaymentResponse(false, GetPaymentGatewayErrorCode(stripeCharge.CancellationReason));
                    }
                    else
                    {
                        return GetPaymentResponse(false, PaymentGatewayError.Unknown);
                    }
                }
                else
                {
                    return GetPaymentResponse(false, PaymentGatewayError.InvalidToken);
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("Your card was declined"))
                {
                    _logger.Log(LogCategory.Error, new Exception("Failed to create charge", ex));
                }
                _transactionPaymentDetailRepository.Save(new TransactionPaymentDetail
                {
                    TransactionId = Convert.ToInt64(stripeChargeParameters.TransactionId),
                    PaymentOptionId = PaymentOptions.CreditCard,
                    PaymentGatewayId = PaymentGateway.Stripe,
                    RequestType = "Charge Recieved",
                    Amount = stripeChargeParameters.Amount.ToString(),
                    PayConfNumber = "",
                    PaymentDetail = "{\"Response\":" + ex.Message + "}",
                });
                return GetPaymentResponse(false, GetPaymentGatewayErrorCode(ex.Message));
            }
        }

        protected PaymentIntent CreateStripeCharge(IStripeCharge stripeChargeParameters)
        {
            var service = new PaymentIntentService();
            var transferOptions = new List<TransferCreateOptions>();
            var chargeOptions = _stripeConnectCharger.StripePaymentIntentCreateOptions(stripeChargeParameters, GetStripeToken(stripeChargeParameters), ref transferOptions);

            PaymentIntent paymentIntent = service.Create(chargeOptions, new RequestOptions
            {
                ApiKey = GetStripeToken(stripeChargeParameters)
            });
            try
            {
                //add transfers to DB scheduled for future. Cronjob will set those up daily.
                if (paymentIntent.Charges.Data.Count() > 0)
                {
                    _stripeConnectCharger.AddFutureTransfers(
                        transferOptions, stripeChargeParameters,
                        paymentIntent.Charges.First().Id,
                        chargeOptions.Currency,
                      (FIL.Contracts.Enums.Channels)stripeChargeParameters.ChannelId);
                }
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, new Exception("stripe connect future transfer", e));
            }
            _transactionPaymentDetailRepository.Save(new TransactionPaymentDetail
            {
                TransactionId = Convert.ToInt64(stripeChargeParameters.TransactionId),
                PaymentOptionId = PaymentOptions.CreditCard,
                PaymentGatewayId = PaymentGateway.Stripe,
                RequestType = "Charge requesting",
                Amount = stripeChargeParameters.Amount.ToString(),
                PayConfNumber = "",
                UserCardDetailId = stripeChargeParameters.UserCardDetailId,
                PaymentDetail = "{\"Request\": {\"Amount\": \"" + chargeOptions.Amount + "\",\"Currency\": \"" + chargeOptions.Currency + "\",\"Description\": \"" + chargeOptions.Description + "\",\"SourceTokenOrExistingSourceId\": \"" + chargeOptions.SourceId + "\",\"StatementDescriptor\": \"" + chargeOptions.StatementDescriptor + "\"}}",
            });
            return paymentIntent;
        }

        protected string GetStripeToken(IStripeCharge stripeChargeParameters)
        {
            if (stripeChargeParameters.ChannelId == Channels.Feel)
            {
                var transactionDetails = _transactionDetailRepository.GetByTransactionId(stripeChargeParameters.TransactionId).FirstOrDefault();
                var liveOnlineTransactionDetailModel = _transactionRepository.GetFeelOnlineDetails(transactionDetails.TransactionId).FirstOrDefault();
                if (liveOnlineTransactionDetailModel != null)
                {
                    var StripeAccount = _eventStripeConnectAccountProvider.GetEventStripeAccount(liveOnlineTransactionDetailModel.EventId, (FIL.Contracts.Enums.Channels)stripeChargeParameters.ChannelId);
                    var @event = _eventRepository.Get(liveOnlineTransactionDetailModel.EventId);
                    if (@event.MasterEventTypeId == Contracts.Enums.MasterEventType.Online
                        && StripeAccount == FIL.Contracts.Enums.StripeAccount.StripeAustralia
                        )
                    {
                        return _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Stripe.FeelAustralia.SecretKey);
                    }
                    else if (@event.MasterEventTypeId == Contracts.Enums.MasterEventType.Online
                     && StripeAccount == FIL.Contracts.Enums.StripeAccount.StripeIndia)
                    {
                        return _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Stripe.FeelIndia.SecretKey);
                    }
                    else if (@event.MasterEventTypeId == Contracts.Enums.MasterEventType.Online
                    && StripeAccount == FIL.Contracts.Enums.StripeAccount.StripeUk)
                    {
                        return _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Stripe.FeelUk.SecretKey);
                    }
                    else if (@event.MasterEventTypeId == Contracts.Enums.MasterEventType.Online
                    && StripeAccount == FIL.Contracts.Enums.StripeAccount.StripeSingapore)
                    {
                        return _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Stripe.FeelISingapore.SecretKey);
                    }
                    else
                    {
                        return _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Stripe.Feel.SecretKey);
                    }
                }
                else
                {
                    return _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Stripe.Feel.SecretKey);
                }
            }

            if (stripeChargeParameters.Currency.ToString().ToUpper() == "AUD" || stripeChargeParameters.Currency.ToString().ToUpper() == "SGD" || stripeChargeParameters.Currency.ToString().ToUpper() == "HKD" || stripeChargeParameters.Currency.ToString().ToUpper() == "NZD")
            {
                return _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Stripe.ZoongaAustralia.SecretKey);
            }
            return _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.Stripe.SecretKey);
        }

        protected PaymentGatewayError GetPaymentGatewayErrorCode(string errorMessage)
        {
            switch (errorMessage)
            {
                case "Your card has expired.": return PaymentGatewayError.ExpiredCard;
                case "Your card has insufficient funds.": return PaymentGatewayError.InsufficientFunds;
                case "Your card number is incorrect.": return PaymentGatewayError.InvalidCardNumber;
                case "Your card does not support this type of purchase.": return PaymentGatewayError.CardNotSupported;
                case string a when a.Contains("Your card was declined"): return PaymentGatewayError.CardDeclined;
                case "Your card's security code is incorrect.": return PaymentGatewayError.InvalidCvv;
                case "Your card's security code is invalid.": return PaymentGatewayError.InvalidCvv;
                case "Invalid amount.": return PaymentGatewayError.InvalidAmount;
                case string a when a.Contains("Invalid currency"): return PaymentGatewayError.InvalidCurrency;
                case string a when a.Contains("Execution Timeout Expired"): return PaymentGatewayError.SessionExpired;
                case "Your card's expiration year is invalid.": return PaymentGatewayError.InvalidExpirationYear;
                case "Your card's expiration month is invalid.": return PaymentGatewayError.InvalidExpirationMonth;
                case string a when a.Contains("You cannot use a Stripe token more than once"): return PaymentGatewayError.InvalidToken;
                case string a when a.Contains("No such token"): return PaymentGatewayError.InvalidToken;
                case string a when a.Contains("card is not supported"): return PaymentGatewayError.CardNotSupported;
                default: return PaymentGatewayError.Unknown;
            }
        }
    }
}