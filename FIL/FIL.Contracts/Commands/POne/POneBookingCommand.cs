using System.Collections.Generic;

namespace FIL.Contracts.Commands.POne
{
    public class POneBookingCommand : BaseCommand
    {
        public List<POneOrderCommandModel> Orders { get; set; }
    }

    public class POneOrderCommandModel
    {
        public long EventTicketAttributeId { get; set; }
        public short? TicketAmount { get; set; }
    }
}