namespace FIL.Contracts.Models
{
    public class TransactionSeatDetail
    {
        public long Id { get; set; }
        public long TransactionDetailId { get; set; }
        public long MatchSeatTicketDetailId { get; set; }
    }
}