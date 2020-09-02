using System.Collections.Generic;

namespace FIL.Contracts.Models
{
    public class TicketLookupSubContainer
    {
        public Event Event { get; set; }
        public List<SubEventContainer> SubEventContainer { get; set; }
    }
}