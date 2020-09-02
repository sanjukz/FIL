namespace FIL.Contracts.Commands.Tiqets
{
    public class SaveOrderCommand : BaseCommand
    {
        public long TransactionId { get; set; }
        public string OrderRefernceId { get; set; }
        public string PaymentToken { get; set; }
    }
}