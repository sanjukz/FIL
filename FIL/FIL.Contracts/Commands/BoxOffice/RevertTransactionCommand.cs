namespace FIL.Contracts.Commands.BoxOffice
{
    public class RevertTransactionCommand : BaseCommand
    {
        public long TransactionId { get; set; }
    }
}