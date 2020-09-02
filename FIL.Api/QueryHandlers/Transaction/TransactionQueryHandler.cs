using FIL.Api.Repositories;
using FIL.Contracts.Queries.Transaction;
using FIL.Contracts.QueryResults.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.Transactions
{
    public class TransactionQueryHandler : IQueryHandler<TransactionQuery, TransactionQueryResult>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionDetailRepository _transactionDetailRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGuestDetailRepository _guestDetailRepository;
        private readonly IASIPaymentResponseDetailRepository _aSIPaymentResponseDetailRepository;
        private readonly IASIPaymentResponseDetailTicketMappingRepository _aSIPaymentResponseDetailTicketMappingRepository;

        public TransactionQueryHandler(IUserRepository userRepository,
            ITransactionDetailRepository transactionDetailRepository,
            IGuestDetailRepository guestDetailRepository,
             IASIPaymentResponseDetailRepository aSIPaymentResponseDetailRepository,
           IASIPaymentResponseDetailTicketMappingRepository aSIPaymentResponseDetailTicketMappingRepository,
            ITransactionRepository transactionRepository)
        {
            _userRepository = userRepository;
            _transactionRepository = transactionRepository;
            _transactionDetailRepository = transactionDetailRepository;
            _aSIPaymentResponseDetailRepository = aSIPaymentResponseDetailRepository;
            _guestDetailRepository = guestDetailRepository;
            _aSIPaymentResponseDetailTicketMappingRepository = aSIPaymentResponseDetailTicketMappingRepository;
        }

        public TransactionQueryResult Handle(TransactionQuery query)
        {
            FIL.Contracts.DataModels.Transaction transaction = new FIL.Contracts.DataModels.Transaction();
            List<FIL.Contracts.DataModels.Transaction> transactions = new List<FIL.Contracts.DataModels.Transaction>();
            Dictionary<long, bool> transactionValue = new Dictionary<long, bool>();
            var isASITicketCreated = true;
            if (query.TransactionId != null && !query.IsASI)
            {
                transaction = _transactionRepository.Get((long)query.TransactionId);
            }
            else if (!string.IsNullOrWhiteSpace(query.Email) && !query.IsASI)
            {
                transaction = _transactionRepository.GetByEmail(query.Email);
            }
            else if (query.IsASI && query.PhoneNumber != null && query.PhoneNumber != "")
            {
                var transactionData = _transactionRepository.GetTransactionByPhoneNumber(query.PhoneNumber);
                transactions = transactionData.Where(s => s.CreatedUtc.Date == DateTime.UtcNow.Date).ToList();
                transaction = transactions.FirstOrDefault();
                foreach (var currentTransaction in transactions)
                {
                    var transactionDetail = _transactionDetailRepository.GetByTransactionId(currentTransaction.Id).ToList();
                    var guestDetails = _guestDetailRepository.GetByTransactionDetailIds(transactionDetail.Select(s => s.Id));
                    var asiTicket = _aSIPaymentResponseDetailTicketMappingRepository.GetByVisitorIds(guestDetails.Select(s => (long)s.Id));
                    if (asiTicket.Count() == 0)
                    {
                        transactionValue.Add(currentTransaction.Id, false);
                    }
                    else
                    {
                        transactionValue.Add(currentTransaction.Id, true);
                    }
                }
            }
            if (transaction == null)
            {
                return new TransactionQueryResult { TransactionIds = transactionValue };
            }
            return new TransactionQueryResult
            {
                TransactionId = transaction.Id,
                TransactionIds = transactionValue,
                AltId = transaction.AltId,
                CurrencyId = transaction.CurrencyId,
                GrossTicketAmount = transaction.GrossTicketAmount,
                DeliveryCharges = transaction.DeliveryCharges,
                ConvenienceCharges = transaction.ConvenienceCharges,
                ServiceCharge = transaction.ServiceCharge,
                DiscountAmount = transaction.DiscountAmount,
                NetTicketAmount = transaction.NetTicketAmount,
                IsASITicketsCreated = isASITicketCreated
            };
        }
    }
}