namespace FIL.Contracts.Commands.SeatLayout
{
    public class BlockCustomerSeatCommand : BaseCommand
    {
        public long MatchLayoutSectionSeatsId { get; set; }
        public bool IsBlock { get; set; }
    }
}