using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults.MasterVenueLayout;

namespace FIL.Contracts.Queries.MasterVenueLayout
{
    public class SaveMasterVenueLayoutQuery : IQuery<SaveMasterVenueLayoutQueryResult>
    {
        public string xmlData { get; set; }
        public int MasterVenueLayoutSectionId { get; set; }
        public bool ShouldSeatInsert { get; set; }
    }
}