namespace FIL.Contracts.Models.Integrations.Kidzania
{
    public class BookResponse
    {
        public long ParkId { get; set; }
        public int ShiftId { get; set; }
        public int NoOfTickets { get; set; }
        public long UniqueRequestId { get; set; }
        public string PartnerId { get; set; }
        public int StatusId { get; set; }
        public string ErrMessage { get; set; }
        public string SaleNumber { get; set; }
        public Ticket[] Tickets { get; set; }
        public double Charge { get; set; }
        public double OtherCharge { get; set; }
        public double ServiceCharge { get; set; }
        public double TotalCharge { get; set; }
    }

    public class Ticket
    {
        public string TicketNumber { get; set; }
        public double Price { get; set; }
    }
}