namespace FIL.Contracts.DataModels
{
    public class Itinerary
    {
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public int CityId { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
    }

    public class FeelState
    {
        public string StateName { get; set; }
        public int StateId { get; set; }
        public string CountryName { get; set; }
    }
}