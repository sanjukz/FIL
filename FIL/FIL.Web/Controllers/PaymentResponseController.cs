using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FIL.Contracts.Commands.Payment;
using FIL.Contracts.Enums;
using FIL.Foundation.Senders;
using FIL.Web.Core.ErrorMessageProviders;
using FIL.Web.Core.Providers;
using FIL.Web.Core.UrlsProvider;
using FIL.Web.Feel.ViewModels.Payment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIL.Web.Feel.Controllers
{
    public class PaymentResponseController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IPaymentErrorMessageProvider _paymentErrorMessageProvider;
        private readonly FIL.Logging.ILogger _logger;
        private readonly ISiteIdProvider _siteIdProvider;
        private readonly ISiteUrlsProvider _siteUrlsProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentResponseController(ICommandSender commandSender, 
            IPaymentErrorMessageProvider paymentErrorMessageProvider,
            ISiteIdProvider siteIdProvider,
            ISiteUrlsProvider siteUrlsProvider,
            IHttpContextAccessor httpContextAccessor,
            FIL.Logging.ILogger logger)
        {
            _commandSender = commandSender;
            _paymentErrorMessageProvider = paymentErrorMessageProvider;
            _siteIdProvider = siteIdProvider;
            _siteUrlsProvider = siteUrlsProvider;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<IActionResult> PaymentResponse()
        {
            string url;
            var siteUrls = _siteUrlsProvider.GetSiteUrl(_siteIdProvider.GetSiteId());
            try
            {
                StringBuilder queryString = new StringBuilder();

                if (!string.IsNullOrWhiteSpace(_httpContextAccessor.HttpContext.Request.QueryString.ToString()))
                {
                    queryString.Append(_httpContextAccessor.HttpContext.Request.QueryString.ToString());
                }
                else
                {
                    if (_httpContextAccessor.HttpContext.Request.Form.Keys.Count > 0)
                    {
                        foreach (string key in _httpContextAccessor.HttpContext.Request.Form.Keys)
                        {
                            queryString.Append($"{key}={_httpContextAccessor.HttpContext.Request.Form[key]}&");
                        }
                    }
                }

                var handleResponse = await HandleResponse(queryString.ToString().TrimEnd('&'));
                if (handleResponse.Success)
                {
                    url = $"{siteUrls}/order-confirmation/{handleResponse.TransactionAltId}";
                }
                else
                {
                    url = $"{siteUrls}/pgerror?{handleResponse.PaymentGatewayError}";
                }
            }
            catch (Exception ex)
            {
                _logger.Log(FIL.Logging.Enums.LogCategory.Error, ex);
                url = $"{siteUrls}/pgerror";
            }
            Response.Redirect(url);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PostBackFormResponse()
        {
            string url;
            string siteUrls = _siteUrlsProvider.GetSiteUrl(_siteIdProvider.GetSiteId());
            try
            {
                StringBuilder queryString = new StringBuilder();
                if (_httpContextAccessor.HttpContext.Request.Form.Keys.Count > 0)
                {
                    foreach (string key in _httpContextAccessor.HttpContext.Request.Form.Keys)
                    {
                        queryString.Append($"{key}={_httpContextAccessor.HttpContext.Request.Form[key]}&");
                    }
                }
                var handleResponse = await HandleResponse(queryString.ToString().TrimEnd('&'));

                if (handleResponse.Success)
                {
                    url = $"{siteUrls}/order-confirmation/{handleResponse.TransactionAltId}";
                }
                else
                {
                    url = $"{siteUrls}/pgerror?{handleResponse.PaymentGatewayError}";
                }
            }
            catch (Exception ex)
            {
                _logger.Log(FIL.Logging.Enums.LogCategory.Error, ex);
                url = $"{siteUrls}/pgerror";

            }
            Response.Redirect(url);
            return View();
        }

        public async Task<PaymentResponseViewModel> HandleResponse(string queryString)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    PaymentResponseCommandResult result =
                        await _commandSender.Send<PaymentResponseCommand, PaymentResponseCommandResult>(
                            new PaymentResponseCommand
                            {
                                TransactionId = 0,
                                PaymentOption = PaymentOptions.CreditCard,
                                QueryString = queryString,
                                ChannelId = Channels.Feel
                            });

                    return new PaymentResponseViewModel
                    {
                        TransactionAltId = result.TransactionAltId,
                        Success = result.PaymentResponse == null ? true : result.PaymentResponse.Success,
                        ErrorMessage = result.PaymentResponse == null
                            ? string.Empty
                            : _paymentErrorMessageProvider.GetPaymentErrorMessage(result.PaymentResponse
                                .PaymentGatewayError),
                        PaymentGatewayError = result.PaymentResponse?.PaymentGatewayError
                    };
                }
                catch (Exception ex)
                {
                    _logger.Log(FIL.Logging.Enums.LogCategory.Error, ex);
                    return new PaymentResponseViewModel
                    {
                        Success = false,
                        ErrorMessage = ex.ToString(),
                        PaymentGatewayError = PaymentGatewayError.Unknown
                    };
                }
            }

            _logger.Log(FIL.Logging.Enums.LogCategory.Warn, "Payment failed due to invalid ModelState", new Dictionary<string, object>
            {
                ["ModelState"] = ModelState
            });
            return new PaymentResponseViewModel { Success = false, PaymentGatewayError = PaymentGatewayError.Unknown };
        }

        [HttpGet]
        [Route("api/payment/error/{err}")]
        public PaymentErrorResponseViewModel GetPaymentErrorMessage(string err)
        {
            var errorMessage = (PaymentGatewayError)Enum.Parse(typeof(PaymentGatewayError), err.Equals("undefined") ? "Unknown" : err);
            return new PaymentErrorResponseViewModel { ErrorDescription = _paymentErrorMessageProvider.GetPaymentErrorMessage(errorMessage) };
        }
    }
}