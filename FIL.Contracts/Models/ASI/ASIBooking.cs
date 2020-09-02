using System.Collections.Generic;

namespace FIL.Contracts.Models.ASI
{
    public class ASIBooking
    {
        public List<FIL.Contracts.Commands.Transaction.EventTicketAttribute> EventTicketAttributeList { get; set; }
        public long TransactionId { get; set; }
        public long UserId { get; set; }
    }
}