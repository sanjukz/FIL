namespace FIL.Contracts.Commands.BoxOffice
{
    public class VoidRequestDetailCommand : BaseCommand

    {
        public long ConfirmationNumber { get; set; }
        public string Reason { get; set; }
        public long UserId { get; set; }
    }
}