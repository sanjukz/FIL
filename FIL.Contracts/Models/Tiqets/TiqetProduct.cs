namespace FIL.Contracts.Models.Tiqets
{
    public class TiqetProduct
    {
        public long Id { get; set; }
        public string ProductId { get; set; }
        public string Tittle { get; set; }
        public string SaleStatus { get; set; }
        public string Inclusions { get; set; }
        public string Language { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }
        public string ProductSlug { get; set; }
        public decimal Price { get; set; }
        public string SaleStatuReason { get; set; }
        public string VenueName { get; set; }
        public string VenueAddress { get; set; }
        public string Summary { get; set; }
        public string TagLine { get; set; }
        public string PromoLabel { get; set; }
        public string RatingAverage { get; set; }
        public string GeoLocationLatitude { get; set; }
        public string GeoLocationLongitude { get; set; }
        public bool IsEnabled { get; set; }
    }
}