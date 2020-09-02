namespace FIL.Contracts.Models.ASI
{
    public class ASITransactionDetailTimeSlotIdMapping
    {
        public long Id { get; set; }
        public long TransactionDetailId { get; set; }
        public long EventTimeSlotMappingId { get; set; }
    }
}