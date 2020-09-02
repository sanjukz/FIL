namespace FIL.Contracts.Models.TMS
{
    public class TicketDetailModel
    {
        public long Id { get; set; }
        public long TransactionId { get; set; }
        public string BarcodeNumber { get; set; }
        public string CategoryName { get; set; }
        public string EventDetailName { get; set; }
        public string VenueName { get; set; }
        public string CityName { get; set; }
        public string EventStartDate { get; set; }
        public string EventStartTime { get; set; }
        public string SeatTag { get; set; }
        public string EntryDate { get; set; }
    }
}