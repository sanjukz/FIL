using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Contracts.Models
{
    public class SubEventContainer
    {
        public Event Event { get; set; }
        public EventAttribute EventAttribute { get; set; }
        public EventDetail EventDetail { get; set; }
        public EventCategory EventCategory { get; set; }
        public Venue Venue { get; set; }
        public City City { get; set; }
        public State State { get; set; }
        public Country Country { get; set; }
        public Zipcode Zipcode { get; set; }
        public IEnumerable<EventTicketDetail> EventTicketDetail { get; set; }
        public IEnumerable<EventTicketAttribute> EventTicketAttribute { get; set; }
        public IEnumerable<EventDeliveryTypeDetail> EventDeliveryTypeDetail { get; set; }
        public IEnumerable<TransactionDetail> TransactionDetail { get; set; }
        public IEnumerable<TicketCategory> TicketCategory { get; set; }
        public IEnumerable<TransactionDeliveryDetail> TransactionDeliveryDetail { get; set; }
        public IEnumerable<MatchLayoutSectionSeat> MatchLayoutSectionSeat { set; get; }
        public IEnumerable<MatchSeatTicketDetail> MatchSeatTicketDetail { get; set; }
        public IEnumerable<MatchLayoutCompanionSeatMapping> MatchLayoutCompanionSeatMappings { get; set; }
        public IEnumerable<FIL.Contracts.Models.ASI.EventTimeSlotMapping> EventTimeSlotMappings { get; set; }
        public IEnumerable<FIL.Contracts.Models.ASI.ASITransactionDetailTimeSlotIdMapping> ASITransactionDetailTimeSlotIdMappings { get; set; }
        public IEnumerable<FIL.Contracts.Models.ASI.ASIPaymentResponseDetailTicketMapping> ASIPaymentResponseDetailTicketMappings { get; set; }
        public IEnumerable<FIL.Contracts.DataModels.TransactionScheduleDetail> TransactionScheduleDetails { get; set; }
        public IEnumerable<FIL.Contracts.DataModels.ScheduleDetail> ScheduleDetails { get; set; }
    }
}