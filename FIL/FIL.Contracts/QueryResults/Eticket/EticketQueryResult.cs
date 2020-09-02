using FIL.Contracts.Enums;

namespace FIL.Contracts.QueryResults.Eticket
{
    public class EticketQueryResult
    {
        public DataModels.Transaction Transaction { get; set; }
        public Models.MatchSeatTicketDetail MatchSeatTicketDetail { get; set; }
        public Models.TransactionDeliveryDetail TransactionDeliveryDetail { get; set; }
        public int? MaxPrintCount { get; set; }
        public DeliveryTypes DeliveryType { get; set; }
        public bool EmailVerified { get; set; }
        public bool IsPAH { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        public bool IsVerified { get; set; }
        public bool Success { get; set; }
    }
}