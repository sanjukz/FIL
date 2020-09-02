using System;

namespace FIL.Contracts.Commands.Transaction
{
    public class TransactionMoveAroundMappingCommand : BaseCommand
    {
        public long TransactionId { get; set; }
        public int EventVenueMappingTimeId { get; set; }
        public MoveAroundBookingAddress PurchaserAddress { get; set; }
        public DateTime CreatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
    }

    public class MoveAroundBookingAddress
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Town { get; set; }
        public string Region { get; set; }
        public int PostalCode { get; set; }
    }
}