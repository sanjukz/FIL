using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FIL.Contracts.Commands.Discount;
using FIL.Contracts.Commands.Payment;
using FIL.Contracts.Enums;
using FIL.Foundation.Senders;
using FIL.Web.Core.ErrorMessageProviders;
using FIL.Web.Core.Providers;
using FIL.Web.Core.UrlsProvider;
using FIL.Web.Feel.ViewModels.Payment;
using Microsoft.AspNetCore.Mvc;

namespace FIL.Web.Feel.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly ISiteUrlsProvider _siteUrlsProvider;
        private readonly ISiteIdProvider _siteIdProvider;
        private readonly IPaymentErrorMessageProvider _paymentErrorMessageProvider;


        public PaymentController(ICommandSender commandSender,
            ISiteUrlsProvider siteUrlsProvider,
            ISiteIdProvider siteIdProvider,
            IPaymentErrorMessageProvider paymentErrorMessageProvider)
        {
            _commandSender = commandSender;
            _siteUrlsProvider = siteUrlsProvider;
            _siteIdProvider = siteIdProvider;
            _paymentErrorMessageProvider = paymentErrorMessageProvider;
        }

        [HttpPost]
        [Route("api/payment")]
        public async Task<PaymentFormResponseViewModel> Charge([FromBody]PaymentFormDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    PaymentCommand paymentQuery;
                    if (model.PaymentGateway == PaymentGateway.Stripe)
                    {
                        paymentQuery = new PaymentCommand
                        {
                            TransactionId = model.TransactionId,
                            PaymentCard = new FIL.Contracts.Commands.Payment.PaymentCard
                            {
                                CardNumber = model.CardNumber,
                                NameOnCard = model.NameOnCard,
                                Cvv = model.Cvv,
                                ExpiryMonth = Convert.ToInt16(model.ExpiryMonth),
                                ExpiryYear = Convert.ToInt16(model.ExpiryYear),
                                CardType = (CardType)model.CardTypeId,
                            },
                            PaymentRedirectUrl = $"{_siteUrlsProvider.GetSiteUrl(_siteIdProvider.GetSiteId())}/PaymentResponse/PaymentResponse?gateway={(int)PaymentGateway.Stripe}&orderId={model.TransactionId}",
                            Token = model.Token,
                            PaymentOption = (PaymentOptions)model.PaymentOption,
                            BillingAddress = new FIL.Contracts.Commands.Payment.BillingAddress
                            {
                                Address = model.Address,
                                Zipcode = model.Zipcode,
                                City = model.City,
                                State = model.State,
                                Country = model.Country
                            },
                            PaymentGateway = PaymentGateway.Stripe,
                            ChannelId = Channels.Feel,
                        };
                    }
                    else
                    {
                        paymentQuery = new PaymentCommand
                        {
                            TransactionId = model.TransactionId,
                            PaymentCard = new FIL.Contracts.Commands.Payment.PaymentCard
                            {
                                CardNumber = model.CardNumber,
                                NameOnCard = model.NameOnCard,
                                Cvv = model.Cvv,
                                ExpiryMonth = Convert.ToInt16(model.ExpiryMonth),
                                ExpiryYear = Convert.ToInt16(model.ExpiryYear),
                                CardType = (CardType)model.CardTypeId,
                            },
                            PaymentOption = (PaymentOptions)model.PaymentOption,
                            BillingAddress = new FIL.Contracts.Commands.Payment.BillingAddress
                            {
                                Address = model.Address,
                                Zipcode = model.Zipcode,
                                City = model.City,
                                State = model.State,
                                Country = model.Country
                            },
                            BankAltId = model.BankAltId,
                            CardAltId = model.CardAltId,
                            PaymentGateway = PaymentGateway.CCAvenue,
                            ChannelId = Channels.Feel,
                            Token = model.Token,
                        };
                    }
                    var result = await _commandSender.Send<PaymentCommand, PaymentCommandResult>(paymentQuery);
                    string _action = result.PaymentHtmlPostResponse == null ? "" : result.PaymentHtmlPostResponse.HtmlPostRequest.Action;
                    string _method = result.PaymentHtmlPostResponse == null ? "" : result.PaymentHtmlPostResponse.HtmlPostRequest.Method;

                    if (model.PaymentGateway == PaymentGateway.Stripe && !result.PaymentResponse.Success && !string.IsNullOrWhiteSpace(result.PaymentResponse.RedirectUrl))
                    {
                        _action = PaymentGatewayError.RequireSourceAction.ToString();
                        _method = result.PaymentResponse.RedirectUrl;
                    }
                    return new PaymentFormResponseViewModel
                    {
                        TransactionAltId = result.TransactionAltId,
                        Success = result.PaymentResponse == null ? true : result.PaymentResponse.Success,
                        ErrorMessage = result.PaymentResponse == null ? "" : result.PaymentResponse.PaymentGatewayError == PaymentGatewayError.None ? "" : result.PaymentResponse.PaymentGatewayError.ToString(),
                        Action = _action,
                        Method = _method,
                        FormFields = result.PaymentHtmlPostResponse == null ? new Dictionary<string, string>() : result.PaymentHtmlPostResponse.HtmlPostRequest.FormFields,
                    };
                }
                catch (Exception ex)
                {
                    return new PaymentFormResponseViewModel { Success = false, ErrorMessage = ex.ToString() };
                }
            }
            return new PaymentFormResponseViewModel { Success = false };
        }

        [HttpPost]
        [Route("api/payment/response")]
        public async Task<PaymentResponseViewModel> HandleResponse([FromBody]PaymentResponseFormDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _commandSender.Send<PaymentResponseCommand, PaymentResponseCommandResult>(new PaymentResponseCommand
                    {
                        TransactionId = model.TransactionId,
                        PaymentOption = (PaymentOptions)model.PaymentOption,
                        QueryString = model.QueryString
                    });

                    return new PaymentResponseViewModel
                    {
                        Success = result.PaymentResponse == null ? true : result.PaymentResponse.Success,
                        ErrorMessage = result.PaymentResponse == null ? "" : _paymentErrorMessageProvider.GetPaymentErrorMessage(result.PaymentResponse.PaymentGatewayError),
                    };


                }
                catch (Exception ex)
                {
                    return new PaymentResponseViewModel { Success = false, ErrorMessage = ex.ToString() };
                }
            }
            else
            {
                return new PaymentResponseViewModel { Success = false };
            }
        }

        [HttpPost]
        [Route("api/applypromocode")]
        public async Task<PromoCodeResponceViewModel> HandleResponsePromocode([FromBody]PromoCodeViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ApplyDiscountCommandResult PromocodeData = await _commandSender.Send<ApplyDiscountCommand, ApplyDiscountCommandResult>(new ApplyDiscountCommand
                    {
                        TransactionId = model.TransactionId,
                        Promocode = model.Promocode,
                        Channel = Channels.Feel
                    });

                    return new PromoCodeResponceViewModel
                    {
                        TransactionId = PromocodeData.Id,
                        IsLimitExceeds = PromocodeData.IsLimitExceeds,
                        CurrencyId = PromocodeData.CurrencyId,
                        GrossTicketAmount = PromocodeData.GrossTicketAmount,
                        DeliveryCharges = PromocodeData.DeliveryCharges,
                        ConvenienceCharges = PromocodeData.ConvenienceCharges,
                        ServiceCharge = PromocodeData.ServiceCharge,
                        DiscountAmount = PromocodeData.DiscountAmount,
                        NetTicketAmount = PromocodeData.NetTicketAmount,
                        Success = PromocodeData.Id > 0 ? true : false,
                        IsPaymentBypass = PromocodeData.IsPaymentBypass
                    };

                }
                catch (Exception e)
                {
                    return new PromoCodeResponceViewModel
                    {
                        TransactionId = 0,
                        Success = false
                    };
                }
            }
            return new PromoCodeResponceViewModel
            {
                TransactionId = 0,
                Success = false
            };
        }
    }
}
