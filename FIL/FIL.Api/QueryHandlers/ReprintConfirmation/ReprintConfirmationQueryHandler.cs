using FIL.Api.Core.Utilities;
using FIL.Api.Repositories;
using FIL.Contracts.Queries.ReprintConfirmation;
using FIL.Contracts.QueryResults.ReprintConfirmation;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.ReprintConfirmation
{
    public class ReprintConfirmationQueryHandler : IQueryHandler<ReprintConfirmationQuery, ReprintConfirmationQueryResult>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IDataSettings _dataSettings;

        public ReprintConfirmationQueryHandler(ITransactionRepository transactionRepository, IDataSettings dataSettings)
        {
            _transactionRepository = transactionRepository;
            _dataSettings = dataSettings;
        }

        public ReprintConfirmationQueryResult Handle(ReprintConfirmationQuery query)
        {
            List<Contracts.DataModels.Transaction> transactions;
            _dataSettings.UnitOfWork.BeginReadUncommittedTransaction();
            try
            {
                if (query.TransactionId == null && query.PhoneNumber == null)
                {
                    transactions = _transactionRepository.GetSuccessFullTransactionByEmail(query.Email).ToList();
                }
                else if (query.PhoneNumber == null && query.Email == null)
                {
                    transactions = _transactionRepository.GetAllByTransactionId((long)query.TransactionId).ToList();
                }
                else
                {
                    transactions = _transactionRepository.GetSuccessFullTransactionByPhone(query.PhoneNumber).ToList();
                }
                return new ReprintConfirmationQueryResult
                {
                    Transaction = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.Transaction>>(transactions).ToList()
                };
            }
            catch (System.Exception ex)
            {
                _dataSettings.UnitOfWork.Rollback();
                return new ReprintConfirmationQueryResult();
            }
        }
    }
}