using System.Collections.Generic;

namespace FIL.Contracts.DataModels
{
    public class CorporateRequestProcData
    {
        public IEnumerable<EventDetail> EventDetail { get; set; }
        public IEnumerable<Event> Event { get; set; }
    }
}