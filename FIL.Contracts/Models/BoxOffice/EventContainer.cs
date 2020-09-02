using System.Collections.Generic;

namespace FIL.Contracts.Models.BoxOffice
{
    public class EventContainer
    {
        public Event Event { get; set; }
        public List<SubEventContainer> subEventContainer { get; set; }
    }
}