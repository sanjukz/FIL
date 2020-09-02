using Dapper;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using FIL.Contracts.Models.Creator;
using System.Data;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface ITransactionsDataRepository : IOrmRepository<TransactionInfo, TransactionInfo>
    {
        TransactionsData GetCustomerInfo(long transactionId, string firstName, string lastName, string emailId, string userMobileNo, string barcodeNumber, bool isFulFilment);
        FILTransactionLocator GetFILTransactionLocator(long transactionId, string firstName, string lastName, string emailId, string userMobileNo);
    }

    public class TransactionsDataRepository : BaseOrmRepository<TransactionInfo>, ITransactionsDataRepository
    {
        public TransactionsDataRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public TransactionsData GetCustomerInfo(
            long transactionId,
            string firstName,
            string lastName,
            string emailId,
            string userMobileNo,
            string barcodeNumber,
            bool isFulFilment)
        {
            var spGetDetails = (string)null;
            TransactionsData transactionsData = new TransactionsData();
            if (isFulFilment)
            {
                spGetDetails = "spGetFulFilmentDataInfo";
            }
            else
            {
                spGetDetails = "spGetTransactionInfo";
            }
            var TransactionInfo = GetCurrentConnection().QueryMultiple(spGetDetails, new
            {
                TransactionId = transactionId,
                FirstName = firstName,
                LastName = lastName,
                EmailId = emailId,
                UserMobileNo = userMobileNo,
                BarcodeNumber = barcodeNumber
            }, commandType: CommandType.StoredProcedure);
            transactionsData.TransactionInfos = TransactionInfo.Read<TransactionInfo>();
            return transactionsData;
        }

        public FILTransactionLocator GetFILTransactionLocator(
            long transactionId,
            string firstName,
            string lastName,
            string emailId,
            string userMobileNo)
        {
            var spGetDetails = "spFILTransactionLocator";
            FILTransactionLocator transactionsData = new FILTransactionLocator();
            var TransactionInfo = GetCurrentConnection().QueryMultiple(spGetDetails, new
            {
                TransactionId = transactionId,
                FirstName = string.IsNullOrEmpty(firstName) ? "0" : firstName,
                LastName = string.IsNullOrEmpty(lastName) ? "0" : lastName,
                EmailId = string.IsNullOrEmpty(emailId) ? "0" : emailId,
                UserMobileNo = string.IsNullOrEmpty(userMobileNo) ? "0" : userMobileNo
            }, commandType: CommandType.StoredProcedure);
            transactionsData.TransactionData = TransactionInfo.Read<FIL.Contracts.Models.Creator.TransactionData>().ToList();
            return transactionsData;
        }
    }
}