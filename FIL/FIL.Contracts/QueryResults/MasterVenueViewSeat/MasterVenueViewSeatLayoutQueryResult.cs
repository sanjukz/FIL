using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.MasterVenueViewSeat
{
    public class MasterVenueViewSeatLayoutQueryResult
    {
        public List<MasterVenueRow> MasterVenueRows { get; set; }
        public bool IsSeatLayout { get; set; }
    }
}