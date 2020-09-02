namespace FIL.Contracts.Models.Algolia
{
    public class AlgoliaCitiesExportModel
    {
        public string ObjectID { get; set; }
        public string CityName { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string PlaceImageUrl { get; set; }
        public long CountryId { get; set; }
        public long CityId { get; set; }
        public long StateId { get; set; }
    }
}