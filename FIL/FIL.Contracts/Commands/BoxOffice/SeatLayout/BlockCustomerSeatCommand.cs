using System.Collections.Generic;

namespace FIL.Contracts.Commands.BoxOffice.SeatLayout
{
    public class BlockCustomerSeatCommand : BaseCommand
    {
        public List<long> MatchLayoutSectionSeatsIds { get; set; }
        public bool IsBlock { get; set; }
    }
}