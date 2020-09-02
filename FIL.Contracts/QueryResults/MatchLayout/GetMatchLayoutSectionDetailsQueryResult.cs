using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.MatchLayout
{
    public class GetMatchLayoutSectionDetailsQueryResult
    {
        public List<SectionDetailsByMatchLayout> SectionDetailsByMatchLayout { get; set; }
        public bool IsExistMatch { get; set; }
        public List<TicketCategory> TicketCategories { get; set; }
        public List<EntryGates> EntryGates { get; set; }
    }
}