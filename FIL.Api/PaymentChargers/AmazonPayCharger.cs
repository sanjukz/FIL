using Com.Amazon.Pwain;
using Com.Amazon.Pwain.Types;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.Enums;
using FIL.Contracts.Models.PaymentChargers;
using FIL.Logging;
using FIL.Logging.Enums;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Web;

namespace FIL.Api.PaymentChargers
{
    public interface IAmazonPayCharger<ICharge, IPaymentRedirectResponse> : IPaymentRedirectCharger<ICharge, IPaymentRedirectResponse>
    {
        IPaymentResponse AmazonPayResponseHandler(IGatewayCharge gatewayCharge);
    }

    public class AmazonPayCharger : PaymentRedirectCharger<ICharge>, IAmazonPayCharger<ICharge, IPaymentRedirectResponse>
    {
        public AmazonPayCharger(ILogger logger, ISettings settings)
            : base(logger, settings)
        {
        }

        protected override async Task<IPaymentRedirectResponse> PaymentRedirectChargeGenerator(ICharge charge)
        {
            try
            {
                MerchantConfiguration merchantConfiguration;
                PWAINBackendSDK pwainBackendSDK;
                Dictionary<String, String> parameters = new Dictionary<string, string>();

                parameters.Add(PWAINConstants.SELLER_ORDER_ID, charge.TransactionId.ToString());
                parameters.Add(PWAINConstants.ORDER_TOTAL_AMOUNT, charge.Amount.ToString());
                parameters.Add(PWAINConstants.ORDER_TOTAL_CURRENCY_CODE, charge.Currency.ToUpper());
                parameters.Add(PWAINConstants.REDIRECT_URL, _settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.AmazonPay.ReturnUrl));

                if (_settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.AmazonPay.IsSandbox).Equals("true"))
                {
                    parameters.Add(PWAINConstants.IS_SANDBOX, "true");
                }

                merchantConfiguration = new MerchantConfiguration.Builder().WithSellerIdValue(_settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.AmazonPay.MerchantId)).WithAwsAccessKeyIdValue(_settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.AmazonPay.AccessKey)).WithAwsSecretKeyIdValue(_settings.GetConfigSetting<string>(SettingKeys.PaymentGateway.AmazonPay.SecretKey)).build();
                pwainBackendSDK = new PWAINBackendSDK(merchantConfiguration);

                string chargeUrl = pwainBackendSDK.GetPaymentUrl(parameters);
                return GetPaymentRedirectResponse(chargeUrl, "");
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to create redirect charge", ex));
                return GetPaymentRedirectResponse(null, ex.Message);
            }
        }

        public IPaymentResponse AmazonPayResponseHandler(IGatewayCharge gatewayCharge)
        {
            NameValueCollection nvcResponseQueryString = HttpUtility.ParseQueryString(gatewayCharge.QueryString);

            var sellerOrderId = nvcResponseQueryString["sellerOrderId"];
            var orderTotalAmount = nvcResponseQueryString["orderTotalAmount"];
            var reasonCode = nvcResponseQueryString["reasonCode"];
            var paymentConfirmationNumber = nvcResponseQueryString["amazonOrderId"];
            var status = nvcResponseQueryString["status"];
            var description = nvcResponseQueryString["description"];

            if (status.ToString().ToUpper() == "SUCCESS")
            {
                if (paymentConfirmationNumber.ToUpper().Equals("FAIL") || string.IsNullOrWhiteSpace(paymentConfirmationNumber))
                {
                    return GetPaymentResponse(false, GetPaymentGatewayErrorCode(reasonCode));
                }
                else
                {
                    return GetPaymentResponse(true, PaymentGatewayError.None);
                }
            }
            else
            {
                return GetPaymentResponse(false, GetPaymentGatewayErrorCode(reasonCode));
            }
        }

        private PaymentGatewayError GetPaymentGatewayErrorCode(string errorCode)
        {
            string errorMessage = string.Empty;
            switch (errorCode)
            {
                case "301": errorMessage = "Simulation data provided is not valid"; return PaymentGatewayError.InvalidData;
                case "230": errorMessage = "Bank response Timed out"; return PaymentGatewayError.NoResonseFromBank;
                case "300": errorMessage = "Invalid Input Parameter from Merchant"; return PaymentGatewayError.InvalidData;
                case "105": errorMessage = "Duplicate seller order id"; return PaymentGatewayError.InvalidOrderId;
                default: return PaymentGatewayError.Unknown;
            }
        }
    }
}