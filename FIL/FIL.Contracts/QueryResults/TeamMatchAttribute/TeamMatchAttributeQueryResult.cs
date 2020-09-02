using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults
{
    public class TeamMatchAttributeQueryResult
    {
        public IEnumerable<EventDetail> EventDetail { get; set; }
        public IEnumerable<Team> Team { get; set; }
        public IEnumerable<MatchAttribute> MatchAttribute { get; set; }
        public IEnumerable<Venue> Venue { get; set; }
        public IEnumerable<City> City { get; set; }
    }
}