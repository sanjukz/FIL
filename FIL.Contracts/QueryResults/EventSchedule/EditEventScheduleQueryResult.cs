using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.EventSchedule
{
    public class EditEventScheduleQueryResult
    {
        public List<FIL.Contracts.Models.EventDetail> SubEvents { get; set; }
        public List<FIL.Contracts.Models.EventDeliveryTypeDetail> DeliveryTypeDetails { get; set; }
        public List<FIL.Contracts.Models.MatchAttribute> MatchAttributes { get; set; }
        public List<FIL.Contracts.Models.EventTicketDetail> EventTicketDetails { get; set; }
        public List<FIL.Contracts.Models.EventTicketAttribute> EventTicketAttributes { get; set; }
        public List<FIL.Contracts.Models.TicketFeeDetail> TicketFeeDetails { get; set; }
    }
}