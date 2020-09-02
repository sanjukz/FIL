using FIL.Api.Repositories;
using FIL.Contracts.Queries.OrderConfirmation;
using FIL.Contracts.QueryResults.OrderConfirmation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.OrderConfirmation
{
    public class TransactionDataQueryHandler : IQueryHandler<TransactionDataQuery, TransactionDataQueryResult>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionDetailRepository _transactionDetailsRepository;
        private readonly IGuestDetailRepository _guestDetailRepository;
        private readonly IASIPaymentResponseDetailTicketMappingRepository _aSIPaymentResponseDetailTicketMappingRepository;
        private readonly IASITransactionDetailTimeSlotIdMappingRepository _aSITransactionDetailTimeSlotIdMappingRepository;

        public TransactionDataQueryHandler(ITransactionRepository transactionRepository,
        ITransactionDetailRepository transactionDetailsRepository,
        IGuestDetailRepository guestDetailRepository,
        IASIPaymentResponseDetailTicketMappingRepository aSIPaymentResponseDetailTicketMappingRepository,
        IASITransactionDetailTimeSlotIdMappingRepository aSITransactionDetailTimeSlotIdMappingRepository
        )
        {
            _transactionRepository = transactionRepository;
            _transactionDetailsRepository = transactionDetailsRepository;
            _guestDetailRepository = guestDetailRepository;
            _aSIPaymentResponseDetailTicketMappingRepository = aSIPaymentResponseDetailTicketMappingRepository;
            _aSITransactionDetailTimeSlotIdMappingRepository = aSITransactionDetailTimeSlotIdMappingRepository;
        }

        public TransactionDataQueryResult Handle(TransactionDataQuery query)
        {
            var transactionIds = query.TransactionIds.Split(",");
            List<long> allTransactionIds = new List<long>();
            foreach (var transactionId in transactionIds)
            {
                allTransactionIds.Add(Convert.ToInt64(transactionId));
            }

            List<FIL.Contracts.DataModels.Transaction> Transactions = new List<Contracts.DataModels.Transaction>();
            List<FIL.Contracts.DataModels.TransactionDetail> TransactionDetails = new List<Contracts.DataModels.TransactionDetail>();
            List<FIL.Contracts.Models.ASI.ASITransactionDetailTimeSlotIdMapping> ASITransactionDetailTimeSlotIdMappings = new List<Contracts.Models.ASI.ASITransactionDetailTimeSlotIdMapping>();
            List<FIL.Contracts.Models.ASI.ASIPaymentResponseDetailTicketMapping> ASIPaymentResponseDetailTicketMappings = new List<Contracts.Models.ASI.ASIPaymentResponseDetailTicketMapping>();
            List<FIL.Contracts.DataModels.GuestDetail> GuestDetails = new List<FIL.Contracts.DataModels.GuestDetail>();

            foreach (var transactionId in transactionIds)
            {
                Transactions = _transactionRepository.GetByTransactionIds(allTransactionIds).ToList();
                TransactionDetails = _transactionDetailsRepository.GetByTransactionIds(Transactions.Select(s => s.Id)).ToList();
                GuestDetails = _guestDetailRepository.GetByTransactionDetailIds(TransactionDetails.Select(s => s.Id)).ToList();
                ASIPaymentResponseDetailTicketMappings = (AutoMapper.Mapper.Map<IEnumerable<FIL.Contracts.Models.ASI.ASIPaymentResponseDetailTicketMapping>>(_aSIPaymentResponseDetailTicketMappingRepository.GetByVisitorIds(GuestDetails.Select(s => (long)s.Id)).ToList())).ToList();
                ASITransactionDetailTimeSlotIdMappings = (AutoMapper.Mapper.Map<IEnumerable<FIL.Contracts.Models.ASI.ASITransactionDetailTimeSlotIdMapping>>(_aSITransactionDetailTimeSlotIdMappingRepository.GetByTransactionDetailIds(TransactionDetails.Select(s => s.Id)))).ToList();
            }
            return new TransactionDataQueryResult
            {
                GuestDetails = GuestDetails,
                ASIPaymentResponseDetailTicketMappings = ASIPaymentResponseDetailTicketMappings,
                ASITransactionDetailTimeSlotIdMappings = ASITransactionDetailTimeSlotIdMappings,
                TransactionDetails = TransactionDetails,
                Transactions = Transactions
            };
        }
    }
}