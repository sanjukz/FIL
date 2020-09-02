namespace FIL.Contracts.Models
{
    public class PlaceTicketRedemptionDetail
    {
        public long Id { get; set; }
        public long EventDetailId { get; set; }
        public string RedemptionsInstructions { get; set; }
        public string RedemptionsAddress { get; set; }
        public string RedemptionsCity { get; set; }
        public string RedemptionsState { get; set; }
        public string RedemptionsCountry { get; set; }
        public string RedemptionsZipcode { get; set; }
        public bool IsEnabled { get; set; }
    }
}