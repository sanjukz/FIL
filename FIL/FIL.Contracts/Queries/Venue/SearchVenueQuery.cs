using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Queries;
using FIL.Contracts.QueryResults;
using System.Collections.Generic;

namespace FIL.Contracts.Queries.Venue
{
    public class SearchVenueQuery : IQuery<SearchVenueQueryResult>
    {
        public string CityName { get; set; }
        public List<int> CityIds { get; set; }
        public string Categories { get; set; }
        public int PlaceId { get; set; }
        public TravelSpeed Speed { get; set; }
        public string SlowPacedDefaultTime { get; set; }
        public string FastPacedDefaultTime { get; set; }
        public string SpendTimeForShop { get; set; }
        public string SpendTimeForEat { get; set; }
        public BudgetRange BudgetRange { get; set; }
    }
}