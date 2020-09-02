namespace FIL.Contracts.Models
{
    public class TransactionPaymentDetail
    {
        public long Id { get; set; }
        public long TransactionId { get; set; }
        public string PaymentOptionId { get; set; }
        public string PaymentGatewayId { get; set; }
        public int UserCardDetailId { get; set; }
        public string RequestType { get; set; }
        public string Amount { get; set; }
        public string PayConfNumber { get; set; }
        public string PaymentDetail { get; set; }
    }
}