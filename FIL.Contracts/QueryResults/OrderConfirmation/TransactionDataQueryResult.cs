using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.OrderConfirmation
{
    public class TransactionDataQueryResult
    {
        public List<FIL.Contracts.DataModels.Transaction> Transactions { get; set; }
        public List<FIL.Contracts.DataModels.TransactionDetail> TransactionDetails { get; set; }
        public List<FIL.Contracts.DataModels.GuestDetail> GuestDetails { get; set; }
        public IEnumerable<FIL.Contracts.Models.ASI.ASITransactionDetailTimeSlotIdMapping> ASITransactionDetailTimeSlotIdMappings { get; set; }
        public IEnumerable<FIL.Contracts.Models.ASI.ASIPaymentResponseDetailTicketMapping> ASIPaymentResponseDetailTicketMappings { get; set; }
    }
}