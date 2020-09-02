using FIL.Contracts.Models.TMS;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.TMS
{
    public class ReleaseSeatCommand : BaseCommand
    {
        public List<SeatDetail> SeatDetails { get; set; }
        //public Guid ModifiedBy { get; set; }
    }
}