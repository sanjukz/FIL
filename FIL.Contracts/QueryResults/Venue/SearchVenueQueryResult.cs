using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults
{
    public class SearchVenueQueryResult
    {
        public List<SearchVenue> Venues { get; set; }
        public List<Days> Days { get; set; }
    }
}