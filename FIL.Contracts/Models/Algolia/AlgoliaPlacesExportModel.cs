namespace FIL.Contracts.Models.Algolia
{
    public class AlgoliaPlacesExportModel
    {
        public string ObjectID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Url { get; set; }
        public string PlaceImageUrl { get; set; }
        public long CountryId { get; set; }
        public long CityId { get; set; }
        public long StateId { get; set; }
    }
}