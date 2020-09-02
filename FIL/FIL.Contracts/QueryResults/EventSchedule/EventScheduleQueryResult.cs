using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.EventSchedule
{
    public class EventScheduleQueryResult
    {
        public List<FIL.Contracts.Models.Event> Events { get; set; }
        public List<FIL.Contracts.Models.Venue> Venues { get; set; }
        public List<FIL.Contracts.Models.Team> Teams { get; set; }
        public List<FIL.Contracts.Models.TicketCategory> TicketCategories { get; set; }
        public List<FeeType> FeeType { get; set; }
        public List<Models.ValueType> ValueType { get; set; }
        public List<FIL.Contracts.Models.CurrencyType> Currencies { get; set; }
    }
}