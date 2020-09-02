using FIL.Api.Repositories;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Contracts.Models.PaymentChargers;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.PaymentChargers
{
    /// <summary>
    /// this class doesnt have any new functionality for stripe charger. Stripe connect can reuse all the logic already in stripecharger.cs .This class will just have the checks and validations needed before we inject these updates in stripe charger and update CreateStripeCharge function to also facilitate StripeConnect. Currently the module only supports those events which will have the connected account in the same region as our stripe account with 1-1 mapping in transaction/event/merchant. (so mostly for zoonga ticketing merchants)
    /// </summary>

    public interface IStripeConnectCharger
    {
        PaymentIntentCreateOptions StripePaymentIntentCreateOptions(IStripeCharge stripeChargeParameters, string apiKey, ref List<TransferCreateOptions> transferOptions);

        void AddFutureTransfers(List<TransferCreateOptions> transferOptions,
            IStripeCharge stripeChargeParameters,
            string chargeId,
            string currencyCode,
            Channels channels);
    }

    public class StripeConnectCharger : IStripeConnectCharger
    {
        private readonly IEventRepository _eventRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;
        private readonly ITransactionStripeConnectTransfersRepository _transactionStripeConnectTransfersRepository;

        public StripeConnectCharger(IEventRepository eventRepository,
            ICurrencyTypeRepository currencyTypeRepository,
            ITransactionStripeConnectTransfersRepository transactionStripeConnectTransfersRepository)
        {
            _eventRepository = eventRepository;
            _currencyTypeRepository = currencyTypeRepository;
            _transactionStripeConnectTransfersRepository = transactionStripeConnectTransfersRepository;
        }

        private IEnumerable<StripeConnectMaster> GetMasterData(long transactionId)
        {
            return _eventRepository.EventStripeConnectMaster(transactionId);
        }

        public void AddFutureTransfers(
            List<TransferCreateOptions> transferOptions,
            IStripeCharge stripeChargeParameters,
            string chargeId,
            string currencyCode,
            Channels channels)
        {
            foreach (TransferCreateOptions transferCreateOption in transferOptions)
            {
                var dict = transferCreateOption.ExtraParams;
                var transferService = new TransferService();
                //remove trasnfer service here and setup API call via schedular in future.
                //this is not done in inject stripeconnect class since i need to also store chargeIDs
                var transactionStripeConnectTransfersRepository = new TransactionStripeConnectTransfers();
                transactionStripeConnectTransfersRepository.Amount = transferCreateOption.Amount.Value;
                transactionStripeConnectTransfersRepository.CreatedBy = (Guid)dict["CreatedBy"];
                transactionStripeConnectTransfersRepository.CreatedUtc = DateTime.UtcNow;
                transactionStripeConnectTransfersRepository.CurrencyId = _currencyTypeRepository.GetByCurrencyCode(currencyCode).Id;
                transactionStripeConnectTransfersRepository.SourceTransactionChargeId = chargeId;
                transactionStripeConnectTransfersRepository.StripeConnectedAccount = transferCreateOption.Destination;
                transactionStripeConnectTransfersRepository.TransactionId = stripeChargeParameters.TransactionId;
                transactionStripeConnectTransfersRepository.TransferDateProposed = (DateTime)dict["EndDateTime"];
                transactionStripeConnectTransfersRepository.ChannelId = channels;
                _transactionStripeConnectTransfersRepository.Save(transactionStripeConnectTransfersRepository);
            }
        }

        public PaymentIntentCreateOptions StripePaymentIntentCreateOptions(IStripeCharge stripeChargeParameters, string apiKey, ref List<TransferCreateOptions> transferOptions)
        {
            IEnumerable<StripeConnectMaster> stripeConnectMaster = GetMasterData(stripeChargeParameters.TransactionId);
            var chargeOptions = new PaymentIntentCreateOptions();
            if (stripeChargeParameters.ChannelId != Channels.Feel)
            {
                long applicationFeeAmount = 0;
                if (stripeConnectMaster != null && stripeConnectMaster.Any())
                    applicationFeeAmount = (long)((stripeConnectMaster.FirstOrDefault().ServiceCharge + stripeConnectMaster.FirstOrDefault().DeliveryCharges + stripeConnectMaster.FirstOrDefault().ConvenienceCharges) * 100);
                if (stripeConnectMaster != null && stripeConnectMaster.Any() && stripeConnectMaster.FirstOrDefault().IsEnabled && applicationFeeAmount != 0)
                {
                    chargeOptions = new PaymentIntentCreateOptions
                    {
                        PaymentMethodId = stripeChargeParameters.Token,
                        Amount = Convert.ToInt32(Convert.ToDouble(stripeChargeParameters.Amount) * 100),
                        Currency = stripeChargeParameters.Currency.ToString().ToLower(),
                        Description = "Transaction charge for " + stripeChargeParameters.TransactionId.ToString(),
                        Confirm = true,
                        ConfirmationMethod = "manual",
                        StatementDescriptor = stripeChargeParameters.ChannelId == Channels.Feel ? "FEELAPLACE.COM" : "ZOONGA.COM"
                    };
                    if (applicationFeeAmount > 0)
                    {
                        var transferOption = new TransferCreateOptions
                        {
                            Amount = (long)(applicationFeeAmount),
                            Currency = stripeChargeParameters.Currency,
                            Destination = stripeConnectMaster.FirstOrDefault().StripeConnectAccountID,
                            ExtraParams = new Dictionary<string, object>() { { "EndDateTime", stripeConnectMaster.FirstOrDefault().EndDateTime.AddDays(stripeConnectMaster.FirstOrDefault().PayoutDaysOffset) }, { "CreatedBy", stripeConnectMaster.FirstOrDefault().CreatedBy } }
                        };
                        transferOptions.Add(transferOption);
                    }
                    return chargeOptions;
                }
            }

            if (stripeChargeParameters.ChannelId == Channels.Feel)
            {
                int chargeCount = 0;
                if (stripeConnectMaster.Count() > 0 && stripeConnectMaster.Where(x => x.IsEnabled == true).Any())
                {
                    chargeOptions = new PaymentIntentCreateOptions
                    {
                        PaymentMethodId = stripeChargeParameters.Token,
                        Amount = Convert.ToInt32(Convert.ToDouble(stripeChargeParameters.Amount) * 100),
                        Currency = stripeChargeParameters.Currency.ToString().ToLower(),
                        Description = "Transaction charge for " + stripeChargeParameters.TransactionId.ToString(),
                        Confirm = true,
                        ConfirmationMethod = "manual",
                        StatementDescriptor = stripeChargeParameters.ChannelId == Channels.Feel ? "FEELAPLACE.COM" : "ZOONGA.COM"
                    };

                    foreach (StripeConnectMaster stripeConnectMasterRow in stripeConnectMaster)
                    {
                        if (stripeConnectMasterRow.IsEnabled)
                        {
                            // Create a Transfer to the connected account for releasing ticket amount (minus our commission):
                            decimal amount = stripeConnectMasterRow.TotalTickets * stripeConnectMasterRow.PricePerTicket * 100;
                            if (stripeConnectMasterRow.ExtraCommisionFlat > 0)
                            {
                                amount = amount - (stripeConnectMasterRow.ExtraCommisionFlat * stripeConnectMasterRow.TotalTickets * 100);
                            }
                            else if (stripeConnectMasterRow.ExtraCommisionPercentage > 0)
                            {
                                amount = amount - ((amount * stripeConnectMasterRow.ExtraCommisionPercentage) / 100);
                            }
                            chargeCount++;
                            if (amount > 0)
                            {
                                var transferOption = new TransferCreateOptions
                                {
                                    Amount = (long)(amount),
                                    Currency = stripeChargeParameters.Currency,
                                    Destination = stripeConnectMasterRow.StripeConnectAccountID,
                                    ExtraParams = new Dictionary<string, object>() { { "EndDateTime", stripeConnectMasterRow.EndDateTime.AddDays(stripeConnectMasterRow.PayoutDaysOffset) }, { "CreatedBy", stripeConnectMasterRow.CreatedBy } }
                                };
                                transferOptions.Add(transferOption);
                            }

                            // Create a Transfer to the connected account for chargeBack amount
                            amount = 0;
                            if (stripeConnectMasterRow.ChargebackHoldFlat > 0)
                            {
                                amount = stripeConnectMasterRow.ChargebackHoldFlat;
                            }
                            else if (stripeConnectMasterRow.ChargebackHoldPercentage > 0)
                            {
                                amount = stripeConnectMasterRow.TotalTickets * stripeConnectMasterRow.PricePerTicket * 100;
                                amount = (amount * stripeConnectMasterRow.ExtraCommisionPercentage) / 100;
                            }

                            if (amount > 0)
                            {
                                var transferOption = new TransferCreateOptions
                                {
                                    Amount = (long)(amount),
                                    Currency = stripeChargeParameters.Currency,
                                    Destination = stripeConnectMasterRow.StripeConnectAccountID,
                                    ExtraParams = new Dictionary<string, object>() { { "EndDateTime", stripeConnectMasterRow.EndDateTime.AddDays(stripeConnectMasterRow.ChargebackDaysOffset) }, { "CreatedBy", stripeConnectMasterRow.CreatedBy } }
                                };
                                transferOptions.Add(transferOption);
                            }
                        }
                    }
                    if (chargeCount > 0)
                    {
                        return chargeOptions;
                    }
                }
            }

            chargeOptions = new PaymentIntentCreateOptions
            {
                PaymentMethodId = stripeChargeParameters.Token,
                Amount = Convert.ToInt32(Convert.ToDouble(stripeChargeParameters.Amount) * 100),
                Currency = stripeChargeParameters.Currency.ToString().ToLower(),
                Description = "Transaction charge for " + stripeChargeParameters.TransactionId.ToString(),
                Confirm = true,
                ConfirmationMethod = "manual",
                StatementDescriptor = stripeChargeParameters.ChannelId == Channels.Feel ? "FEELAPLACE.COM" : "ZOONGA.COM"
            };

            return chargeOptions;
        }
    }
}