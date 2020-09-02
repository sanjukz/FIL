using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.Boxoffice.SeatLayout
{
    public class BlockByCustomerQueryResult
    {
        public List<long> AlreadyBookedSeats { get; set; }
        public List<long> AvailabelSeats { get; set; }
    }
}