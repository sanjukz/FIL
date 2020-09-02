namespace FIL.Contracts.QueryResults.MasterVenueLayoutSections
{
    public class MasterVenueLayoutSectionsQueryResult
    {
        public bool IsExisting { get; set; }
        public bool IsSeatExists { get; set; }
        public int AvailableCapacity { get; set; }
        public bool IsNotCapacityAvailable { get; set; }
    }
}