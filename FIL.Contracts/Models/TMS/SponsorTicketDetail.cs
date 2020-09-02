namespace FIL.Contracts.Models.TMS
{
    public class SponsorTicketDetail
    {
        public long TransactionId { get; set; }
        public int Quantity { get; set; }
        public int TotalTickets { get; set; }
        public string SerialStart { get; set; }
        public string SerialEnd { get; set; }
        public string TicketHandedBy { get; set; }
        public string TicketHandedTo { get; set; }
    }
}