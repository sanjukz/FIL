using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.TicketPrint
{
    public class TicketPrintQueryResult
    {
        public IEnumerable<TicketContainer> TicketContainers { get; set; }
    }

    public class TicketContainer
    {
        public Event Event { get; set; }
        public EventDetail EventDetail { get; set; }
        public Venue Venue { get; set; }
        public City City { get; set; }
        public EventAttribute EventAttribute { get; set; }

        //  public EntryGate EntryGate { get; set; }
        public MatchLayoutSection MatchLayoutSection { get; set; }

        public MatchLayoutSectionSeat MatchLayoutSectionSeat { get; set; }
        public MatchSeatTicketDetail MatchSeatTicketDetail { get; set; }
        public FIL.Contracts.DataModels.Transaction Transaction { get; set; }
        public TransactionDetail TransactionDetail { get; set; }
        public EventTicketAttribute EventTicketAttribute { get; set; }
        public EventTicketDetail EventTicketDetail { get; set; }
    }
}