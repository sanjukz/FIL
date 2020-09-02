namespace FIL.Contracts.Commands.BoxOffice
{
    public class SaveFulFilmentDetailCommand : BaseCommand
    {
        public long? TransactionDetailId { get; set; }
        public string TicketNumber { get; set; }
    }
}