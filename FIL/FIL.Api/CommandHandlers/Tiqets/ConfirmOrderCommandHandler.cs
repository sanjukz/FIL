using FIL.Api.Repositories.Tiqets;
using FIL.Api.Utilities;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.Commands.Tiqets;
using FIL.Contracts.DataModels.Tiqets;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using FIL.Contracts.Models.Tiqets;
using FIL.Logging;
using FIL.Logging.Enums;
using MediatR;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.Tiqets
{
    public class ConfirmOrderCommandHandler : BaseCommandHandlerWithResult<ConfirmOrderCommand, ConfirmOrderCommandResult>
    {
        private readonly ILogger _logger;
        private readonly ITiqetsTransactionRepository _tiqetsTransactionRepository;
        private readonly ISettings _settings;

        public ConfirmOrderCommandHandler(
       ILogger logger, ISettings settings, ITiqetsTransactionRepository tiqetsTransactionRepository,
        IMediator mediator) : base(mediator)
        {
            _logger = logger;
            _settings = settings;
            _tiqetsTransactionRepository = tiqetsTransactionRepository;
        }

        protected override async Task<ICommandResult> Handle(ConfirmOrderCommand command)
        {
            ConfirmOrderCommandResult confirmOrderResult = new ConfirmOrderCommandResult();
            var tiqetsTransaction = _tiqetsTransactionRepository.GetByTransactionId(command.TransactionId);
            if (!tiqetsTransaction.IsOrderConfirmed)
            {
                var confirmOrder = await ConfirmOrder(tiqetsTransaction);
                var responseJson = Mapper<ConfirmOrderResponseModel>.MapFromJson(confirmOrder);
                if (responseJson.success)
                {
                    var getTickets = await GetTickets(responseJson.order_reference_id);
                    if (getTickets.success)
                    {
                        tiqetsTransaction.IsOrderConfirmed = true;
                        tiqetsTransaction.TicketPdf = getTickets.tickets_pdf_url;
                        tiqetsTransaction.PostPurchaseInfo = getTickets.post_purchase_info;
                        tiqetsTransaction.HowToUse = getTickets.how_to_use_info;
                        switch (getTickets.order_status)
                        {
                            case "new":
                                tiqetsTransaction.OrderStatus = Convert.ToInt16(TiqetOrderStatus.New);
                                break;

                            case "pending":
                                tiqetsTransaction.OrderStatus = Convert.ToInt16(TiqetOrderStatus.Pending);
                                break;

                            case "failed":
                                tiqetsTransaction.OrderStatus = Convert.ToInt16(TiqetOrderStatus.Failed);
                                break;

                            case "done":
                                tiqetsTransaction.OrderStatus = Convert.ToInt16(TiqetOrderStatus.Done);
                                break;

                            default:
                                tiqetsTransaction.OrderStatus = Convert.ToInt16(TiqetOrderStatus.None);
                                break;
                        }
                        _tiqetsTransactionRepository.Save(tiqetsTransaction);
                        confirmOrderResult.Success = true;
                        confirmOrderResult.TicketPdfLink = getTickets.tickets_pdf_url;
                    }
                }
                else
                {
                    confirmOrderResult.Success = false;
                }
            }
            else
            {
                confirmOrderResult.Success = true;
                confirmOrderResult.TicketPdfLink = tiqetsTransaction.TicketPdf;
            }
            return confirmOrderResult;
        }

        public async Task<string> ConfirmOrder(TiqetsTransaction tiqetsTransaction)
        {
            string token = GetToken(tiqetsTransaction, _settings.GetConfigSetting<string>(SettingKeys.Integration.Tiqets.PrivateKey));
            try
            {
                var baseAddress = new Uri(_settings.GetConfigSetting<string>(SettingKeys.Integration.Tiqets.Endpoint));
                string responseData;
                using (var httpClient = new HttpClient { BaseAddress = baseAddress })
                {
                    httpClient.Timeout = new TimeSpan(1, 0, 0);
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", _settings.GetConfigSetting<string>(SettingKeys.Integration.Tiqets.Token));
                    var content = new StringContent(token, Encoding.Default, "text/plain");
                    var response = await httpClient.PutAsync("orders/" + tiqetsTransaction.OrderReferenceId + "?lang=en", content);
                    responseData = await response.Content.ReadAsStringAsync();
                    return responseData;
                }
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to get Tiqets time slots", e));
                return null;
            }
        }

        public static string GetToken(TiqetsTransaction tiqetsTransaction, string privateKey)
        {
            ConfirmOrderRequestModel requestModel = new ConfirmOrderRequestModel();
            requestModel.payment_confirmation_token = tiqetsTransaction.PaymentConfirmationToken;
            RSAParameters rsaParams;
            using (var tr = new StringReader(privateKey))
            {
                var pemReader = new PemReader(tr);
                var keyPair = pemReader.ReadObject() as AsymmetricCipherKeyPair;
                if (keyPair == null)
                {
                    throw new Exception("Could not read RSA private key");
                }
                var privateRsaParams = keyPair.Private as RsaPrivateCrtKeyParameters;
                rsaParams = DotNetUtilities.ToRSAParameters(privateRsaParams);
            }
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(rsaParams);
                var json = JsonConvert.SerializeObject(requestModel);
                return Jose.JWT.Encode(json, rsa, Jose.JwsAlgorithm.RS256);
            }
        }

        public async Task<GetTicketResponseModel> GetTickets(string orderReferenceId)
        {
            try
            {
                var baseAddress = new Uri(_settings.GetConfigSetting<string>(SettingKeys.Integration.Tiqets.Endpoint));
                string responseData;
                using (var httpClient = new HttpClient { BaseAddress = baseAddress })
                {
                    httpClient.Timeout = new TimeSpan(1, 0, 0);
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", _settings.GetConfigSetting<string>(SettingKeys.Integration.Tiqets.Token));
                    using (var response = await httpClient.GetAsync("orders/" + orderReferenceId + "/tickets?lang=en"))
                    {
                        responseData = await response.Content.ReadAsStringAsync();
                    }
                }
                var responseJson = Mapper<GetTicketResponseModel>.MapFromJson(responseData);
                return responseJson;
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to get Tiqets time slots", e));
                return null;
            }
        }
    }
}