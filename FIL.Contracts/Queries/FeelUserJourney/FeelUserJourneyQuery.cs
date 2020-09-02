using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.FeelUserJourney;

namespace FIL.Contracts.Queries.FeelUserJourney
{
    public class FeelUserJourneyQuery : IQuery<FeelUserJourneyQueryResult>
    {
        public PageType PageType { get; set; }
        public string PagePath { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public FIL.Contracts.Enums.MasterEventType MasterEventType { get; set; }
    }
}