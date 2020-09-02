using FIL.Contracts.DataModels;

namespace FIL.Contracts.QueryResults.Description
{
    public class DescriptionQueryResult
    {
        public City City { get; set; }
        public FIL.Contracts.DataModels.Country Country { get; set; }
        public FIL.Contracts.DataModels.CityDescription CityDescription { get; set; }
        public FIL.Contracts.DataModels.CountryDescription CountryDescription { get; set; }
        public FIL.Contracts.DataModels.StateDescription StateDescription { get; set; }
    }
}