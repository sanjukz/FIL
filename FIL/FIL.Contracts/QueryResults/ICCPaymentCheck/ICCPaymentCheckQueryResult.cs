namespace FIL.Contracts.QueryResults.ICCPaymentCheck
{
    public class ICCPaymentCheckQueryResult
    {
        public bool IsTicketLimitExceed { get; set; }
        public string TicketLimitErrorBy { get; set; }
        public FIL.Contracts.Models.EventDetail eventDetails { get; set; }
    }
}