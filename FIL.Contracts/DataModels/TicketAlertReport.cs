namespace FIL.Contracts.DataModels
{
    public class TicketAlertReport
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string SubEventName { get; set; }
        public string CountryName { get; set; }
        public int TicketCount { get; set; }
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
    }
}