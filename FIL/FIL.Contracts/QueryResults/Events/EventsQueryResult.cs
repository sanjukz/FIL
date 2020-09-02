using System;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.Events
{
    public class EventsQueryResult
    {
        public List<EventsSiteMap> EventsURLs { get; set; }
    }

    public class EventsSiteMap
    {
        public Guid EventAltId { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
    }
}