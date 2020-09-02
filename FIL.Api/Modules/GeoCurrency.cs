using CurrencyConverter;
using FIL.Api.QueryHandlers;
using FIL.Contracts.Commands.Transaction;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.CurrencyTypes;
using FIL.Contracts.QueryResults;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Modules.SiteExtensions
{
    public interface IGeoCurrency
    {
        CurrencyType GetCurrencyCode(int currencyID);

        CurrencyType GetCurrencyID(string currencyCode);

        UpdateTransactionCommandResult UpdateTransactionUpdates(UpdateTransactionCommandResult updateTransactionCommandResult, string TargetCurrencyCode);

        FIL.Contracts.DataModels.TransactionDetail UpdateTransactionUpdates(FIL.Contracts.DataModels.TransactionDetail updateTransactionCommandResult, string TargetCurrencyCode, int currencyID);

        FIL.Contracts.DataModels.Transaction UpdateTransactionUpdates(FIL.Contracts.DataModels.Transaction updateTransactionCommandResult, string TargetCurrencyCodecontext);

        Decimal GetConvertedDiscountAmount(Decimal discountAmount, int currentCurrencyId, string TargetCurrencyCode);

        List<FIL.Contracts.Models.TicketFeeDetail> UpdateTicketFeeDetails(List<FIL.Contracts.Models.TicketFeeDetail> ticketFeeDetails, string TargetCurrencyCode, int currencyID);
    }

    public class GeoCurrency : IGeoCurrency
    {
        private readonly bool _IsGeoCurrencySelectionEnabled = true;
        private readonly IQueryHandler<CurrencyTypesQuery, CurrencyTypesQueryResult> _querySender;
        private readonly ICurrencyConverter _currencyConverter;

        public GeoCurrency(IQueryHandler<CurrencyTypesQuery, CurrencyTypesQueryResult> querySender, IMemoryCache memoryCache,
            ICurrencyConverter currencyConverter)
        {
            _querySender = querySender;
            _currencyConverter = currencyConverter;
        }

        public CurrencyType GetCurrencyCode(int currencyID)
        {
            var queryResult = GetCurrencyTypesQueryResult();
            Contracts.Models.CurrencyType _ct = queryResult.currencyTypes.Where(x => x.Id == currencyID).FirstOrDefault();
            return _ct;
        }

        public CurrencyType GetCurrencyID(string currencyCode)
        {
            var queryResult = GetCurrencyTypesQueryResult();
            Contracts.Models.CurrencyType _ct = queryResult.currencyTypes.Where(x => x.Code == currencyCode).FirstOrDefault();
            return _ct;
        }

        private CurrencyTypesQueryResult GetCurrencyTypesQueryResult()
        {
            return _querySender.Handle(new CurrencyTypesQuery { });
        }

        public UpdateTransactionCommandResult UpdateTransactionUpdates(UpdateTransactionCommandResult updateTransactionCommandResult, string TargetCurrencyCode)
        {
            if (_IsGeoCurrencySelectionEnabled && updateTransactionCommandResult != null)
            {
                int TargetCurrencyID = GetCurrencyID(TargetCurrencyCode).Id;
                if (updateTransactionCommandResult.GrossTicketAmount.HasValue)
                    updateTransactionCommandResult.GrossTicketAmount = _currencyConverter.Exchange(updateTransactionCommandResult.GrossTicketAmount.Value, GetCurrencyCode(updateTransactionCommandResult.CurrencyId).Code, TargetCurrencyCode);

                if (updateTransactionCommandResult.DeliveryCharges.HasValue)
                    updateTransactionCommandResult.DeliveryCharges = _currencyConverter.Exchange(updateTransactionCommandResult.DeliveryCharges.Value, GetCurrencyCode(updateTransactionCommandResult.CurrencyId).Code, TargetCurrencyCode);

                if (updateTransactionCommandResult.ConvenienceCharges.HasValue)
                    updateTransactionCommandResult.ConvenienceCharges = _currencyConverter.Exchange(updateTransactionCommandResult.ConvenienceCharges.Value, GetCurrencyCode(updateTransactionCommandResult.CurrencyId).Code, TargetCurrencyCode);

                if (updateTransactionCommandResult.ServiceCharge.HasValue)
                    updateTransactionCommandResult.ServiceCharge = _currencyConverter.Exchange(updateTransactionCommandResult.ServiceCharge.Value, GetCurrencyCode(updateTransactionCommandResult.CurrencyId).Code, TargetCurrencyCode);

                if (updateTransactionCommandResult.DiscountAmount.HasValue)
                    updateTransactionCommandResult.DiscountAmount = _currencyConverter.Exchange(updateTransactionCommandResult.DiscountAmount.Value, GetCurrencyCode(updateTransactionCommandResult.CurrencyId).Code, TargetCurrencyCode);

                if (updateTransactionCommandResult.NetTicketAmount.HasValue)
                    updateTransactionCommandResult.NetTicketAmount = _currencyConverter.Exchange(updateTransactionCommandResult.NetTicketAmount.Value, GetCurrencyCode(updateTransactionCommandResult.CurrencyId).Code, TargetCurrencyCode);

                updateTransactionCommandResult.CurrencyId = TargetCurrencyID; ;
            }

            return updateTransactionCommandResult;
        }

        public List<FIL.Contracts.Models.TicketFeeDetail> UpdateTicketFeeDetails(List<FIL.Contracts.Models.TicketFeeDetail> ticketFeeDetails, string TargetCurrencyCode, int currencyId)
        {
            if (_IsGeoCurrencySelectionEnabled && ticketFeeDetails.Any())
            {
                foreach (FIL.Contracts.Models.TicketFeeDetail ticketFeel in ticketFeeDetails)
                {
                    if (ticketFeel.ValueTypeId == 2) // Convert only if value type is flat not percentage...
                    {
                        int TargetCurrencyID = GetCurrencyID(TargetCurrencyCode).Id;
                        ticketFeel.Value = _currencyConverter.Exchange(ticketFeel.Value, GetCurrencyCode(currencyId).Code, TargetCurrencyCode);
                    }
                }
            }
            return ticketFeeDetails;
        }

        public FIL.Contracts.DataModels.TransactionDetail UpdateTransactionUpdates(FIL.Contracts.DataModels.TransactionDetail transactionDetail, string TargetCurrencyCode, int currencyId)
        {
            if (_IsGeoCurrencySelectionEnabled && transactionDetail != null)
            {
                int TargetCurrencyID = GetCurrencyID(TargetCurrencyCode).Id;
                if (transactionDetail.ConvenienceCharges.HasValue)
                    transactionDetail.ConvenienceCharges = _currencyConverter.Exchange(transactionDetail.ConvenienceCharges.Value, GetCurrencyCode(currencyId).Code, TargetCurrencyCode);

                if (transactionDetail.DeliveryCharges.HasValue)
                    transactionDetail.DeliveryCharges = _currencyConverter.Exchange(transactionDetail.DeliveryCharges.Value, GetCurrencyCode(currencyId).Code, TargetCurrencyCode);

                if (transactionDetail.DiscountAmount.HasValue)
                    transactionDetail.DiscountAmount = _currencyConverter.Exchange(transactionDetail.DiscountAmount.Value, GetCurrencyCode(currencyId).Code, TargetCurrencyCode);

                transactionDetail.PricePerTicket = _currencyConverter.Exchange(transactionDetail.PricePerTicket, GetCurrencyCode(currencyId).Code, TargetCurrencyCode);

                if (transactionDetail.ServiceCharge.HasValue)
                    transactionDetail.ServiceCharge = _currencyConverter.Exchange(transactionDetail.ServiceCharge.Value, GetCurrencyCode(currencyId).Code, TargetCurrencyCode);
            }
            return transactionDetail;
        }

        public FIL.Contracts.DataModels.Transaction UpdateTransactionUpdates(FIL.Contracts.DataModels.Transaction transaction, string TargetCurrencyCode)
        {
            if (_IsGeoCurrencySelectionEnabled && transaction != null)
            {
                int TargetCurrencyID = GetCurrencyID(TargetCurrencyCode).Id;
                if (transaction.ConvenienceCharges.HasValue)
                    transaction.ConvenienceCharges = _currencyConverter.Exchange(transaction.ConvenienceCharges.Value, GetCurrencyCode(transaction.CurrencyId).Code, TargetCurrencyCode);

                if (transaction.DeliveryCharges.HasValue)
                    transaction.DeliveryCharges = _currencyConverter.Exchange(transaction.DeliveryCharges.Value, GetCurrencyCode(transaction.CurrencyId).Code, TargetCurrencyCode);

                if (transaction.DiscountAmount.HasValue)
                    transaction.DiscountAmount = _currencyConverter.Exchange(transaction.DiscountAmount.Value, GetCurrencyCode(transaction.CurrencyId).Code, TargetCurrencyCode);

                if (transaction.GrossTicketAmount.HasValue)
                    transaction.GrossTicketAmount = _currencyConverter.Exchange(transaction.GrossTicketAmount.Value, GetCurrencyCode(transaction.CurrencyId).Code, TargetCurrencyCode);

                if (transaction.NetTicketAmount.HasValue)
                    transaction.NetTicketAmount = _currencyConverter.Exchange(transaction.NetTicketAmount.Value, GetCurrencyCode(transaction.CurrencyId).Code, TargetCurrencyCode);

                if (transaction.ServiceCharge.HasValue)
                    transaction.ServiceCharge = _currencyConverter.Exchange(transaction.ServiceCharge.Value, GetCurrencyCode(transaction.CurrencyId).Code, TargetCurrencyCode);

                transaction.CurrencyId = TargetCurrencyID;
            }
            return transaction;
        }

        public Decimal GetConvertedDiscountAmount(Decimal discountAmount, int currentCurrencyId, string TargetCurrencyCode)
        {
            discountAmount = _currencyConverter.Exchange(discountAmount, GetCurrencyCode(currentCurrencyId).Code, TargetCurrencyCode);
            return discountAmount;
        }
    }
}