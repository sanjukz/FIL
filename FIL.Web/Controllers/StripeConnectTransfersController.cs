using FIL.Contracts.DataModels;
using FIL.Foundation.Senders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FIL.Contracts.Queries.TransactionStripeConnectTransfers;
using FIL.Contracts.QueryResults.TransactionStripeConnectTransfers;
using FIL.Contracts.Queries.CurrencyTypes;
using FIL.Contracts.QueryResults;
using Stripe;
using FIL.Configuration;
using FIL.Contracts.Commands.TransactionStripeConnectTransfers;
using FIL.Logging;
using FIL.Logging.Enums;
using Microsoft.AspNetCore.Mvc;
using FIL.Configuration;

namespace FIL.Web.Feel.Controllers
{
    public class StripeConnectTransfersController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IQuerySender _querySender;
        private readonly ILogger _logger;
        private readonly ISettings _settings;

        public StripeConnectTransfersController(ICommandSender commandSender, IQuerySender querySender, ISettings settings, ILogger logger)
        {
            _commandSender = commandSender;
            _querySender = querySender;
            _settings = settings;
            _logger = logger;
        }

        [HttpGet]
        [Route("api/StripeConnectTransfers")]
        public async Task StripeConnectTransfersControllerApi()
        {
            _logger.Log(LogCategory.Debug,"Starting job");
            try
            {
                //start job here.
                var currenyList = await _querySender.Send(new CurrencyTypesQuery { });
                var queryResults = await _querySender.Send(new TransactionStripeConnectTransfersQuery { TransferDate = DateTime.Today });
                foreach (TransactionStripeConnectTransfer transactionStripeConnectTransfer in queryResults.transactionStripeConnectTransfers)
                {
                    var currency = currenyList.currencyTypes.Where(x => x.Id == transactionStripeConnectTransfer.CurrencyId).First().Code;
                    if (currency.ToUpper() == "AUD" || currency.ToUpper() == "USD")
                    {
                        _logger.Log(LogCategory.Debug, "starting transfer for " + transactionStripeConnectTransfer.Id.ToString());
                        // Create a PaymentIntent:
                        Transfer transfer = new Transfer();
                        try
                        {
                            var transferService = new TransferService();
                            var transferOption = new TransferCreateOptions
                            {
                                Amount = (long)(transactionStripeConnectTransfer.Amount),
                                Currency = currenyList.currencyTypes.Where(x => x.Id == transactionStripeConnectTransfer.CurrencyId).First().Code,
                                Destination = transactionStripeConnectTransfer.StripeConnectedAccount,
                                SourceTransaction = transactionStripeConnectTransfer.SourceTransactionChargeId
                            };                            
                            
                            var apiKey = _settings.GetConfigSetting(FIL.Configuration.Utilities.SettingKeys.PaymentGateway.Stripe.SecretKey).Value;
                            if (currency.ToUpper() == "AUD")
                                apiKey = _settings.GetConfigSetting(FIL.Configuration.Utilities.SettingKeys.PaymentGateway.Stripe.FeelAustralia.SecretKey).Value;
                            transfer = transferService.Create(transferOption, new RequestOptions
                            {
                                ApiKey=apiKey
                            });
                            _logger.Log(LogCategory.Debug, "transfer completed for " + transactionStripeConnectTransfer.Id.ToString());
                            await _commandSender.Send<TransactionStripeConnectTransfersCommand>(new TransactionStripeConnectTransfersCommand { Id = transactionStripeConnectTransfer.Id, TransferApiResponse = transfer.ToString(), TransferDateActual = DateTime.Today });
                        }
                        catch (Exception ex)
                        {
                            _logger.Log(LogCategory.Error, ex);
                            await _commandSender.Send<TransactionStripeConnectTransfersCommand>(new TransactionStripeConnectTransfersCommand { Id = transactionStripeConnectTransfer.Id, TransferApiResponse = ex.ToString(), TransferDateActual = DateTime.Today });
                        }
                    }
                    else
                    {
                        await _commandSender.Send<TransactionStripeConnectTransfersCommand>(new TransactionStripeConnectTransfersCommand { Id = transactionStripeConnectTransfer.Id, TransferApiResponse = "cross region account.", TransferDateActual = DateTime.Today });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, ex);
            }
            _logger.Log(LogCategory.Debug, "Finishing Job.");
        }
        public IActionResult Index()
        {            
            return View();
        }
    }
}