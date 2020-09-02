namespace FIL.Contracts.Commands.OrderConfirmationSuccess
{
    public class OrderConfirmationSuccessCommand : BaseCommand
    {
        public long TransactionId { get; set; }
    }
}