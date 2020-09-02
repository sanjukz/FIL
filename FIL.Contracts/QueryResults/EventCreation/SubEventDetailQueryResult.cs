using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.EventCreation
{
    public class SubEventDetailQueryResult
    {
        public IEnumerable<EventDetail> EventDetail { get; set; }
    }
}