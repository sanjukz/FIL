using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.Reporting;
using FIL.Contracts.QueryResults.Reporting;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.TransactionReport
{
    public class ReportTransactionDataQueryHandler : IQueryHandler<ReportTransactionDataQuery, ReportTransactionDataQueryResult>
    {
        private readonly IReportingRepository _reportingRepository;

        public ReportTransactionDataQueryHandler(IReportingRepository reportingRepository)
        {
            _reportingRepository = reportingRepository;
        }

        public ReportTransactionDataQueryResult Handle(ReportTransactionDataQuery query)
        {
            FIL.Contracts.DataModels.ReportTransactionData reportTransactionData = new FIL.Contracts.DataModels.ReportTransactionData();
            List<FIL.Contracts.Models.Transaction> transactions = new List<FIL.Contracts.Models.Transaction>();
            List<FIL.Contracts.Models.TransactionDetail> transactionDetails = new List<FIL.Contracts.Models.TransactionDetail>();
            List<TransactionPaymentDetail> transactionPaymentDetails = new List<TransactionPaymentDetail>();
            List<TransactionDeliveryDetail> transactionDeliveryDetails = new List<TransactionDeliveryDetail>();
            List<CurrencyType> currenyTypes = new List<CurrencyType>();
            List<User> users = new List<User>();
            List<FIL.Contracts.Models.IPDetail> ipDetails = new List<FIL.Contracts.Models.IPDetail>();
            List<UserCardDetail> userCardDetails = new List<UserCardDetail>();

            try
            {
                //reportTransactionData = _reportingRepository.GetReportTransactionData(query);
                //transactions = AutoMapper.Mapper.Map<List<Kz.Contracts.Models.Transaction>>(reportTransactionData.Transaction);
                //transactionDetails = AutoMapper.Mapper.Map<List<Kz.Contracts.Models.TransactionDetail>>(reportTransactionData.TransactionDetail);
                //transactionPaymentDetails = AutoMapper.Mapper.Map<List<TransactionPaymentDetail>>(reportTransactionData.TransactionPaymentDetail);
                //transactionDeliveryDetails = AutoMapper.Mapper.Map<List<TransactionDeliveryDetail>>(reportTransactionData.TransactionDeliveryDetail);
                //currenyTypes = AutoMapper.Mapper.Map<List<CurrencyType>>(reportTransactionData.CurrencyType);
                //users = AutoMapper.Mapper.Map<List<User>>(reportTransactionData.User);
                //ipDetails = AutoMapper.Mapper.Map<List<IPDetail>>(reportTransactionData.IPDetail);
                //userCardDetails = AutoMapper.Mapper.Map<List<UserCardDetail>>(reportTransactionData.UserCardDetail);

                return new ReportTransactionDataQueryResult
                {
                    Transaction = transactions,
                    TransactionDetail = transactionDetails.OrderByDescending(o => o.TransactionId).ToList(),
                    TransactionDeliveryDetail = transactionDeliveryDetails,
                    TransactionPaymentDetail = transactionPaymentDetails.Distinct().ToList(),
                    CurrencyType = currenyTypes,
                    User = users,
                    IPDetail = ipDetails,
                    UserCardDetail = userCardDetails,
                    //Pagination = reportTransactionData.Pagination.FirstOrDefault()
                    Pagination = new Pagination()
                };
            }
            catch (System.Exception ex)
            {
                return new ReportTransactionDataQueryResult
                {
                    Transaction = transactions,
                    TransactionDetail = transactionDetails,
                    TransactionDeliveryDetail = transactionDeliveryDetails,
                    TransactionPaymentDetail = transactionPaymentDetails.Distinct().ToList(),
                    CurrencyType = currenyTypes,
                    User = users,
                    IPDetail = ipDetails,
                    UserCardDetail = userCardDetails,
                    Pagination = new Pagination()
                };
            }
        }
    }
}