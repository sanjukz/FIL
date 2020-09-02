using System.Collections.Generic;
using static FIL.Contracts.Models.Integrations.ExOz.ExOzGeolocationResponse;

namespace FIL.Contracts.Models.Integrations.ExOz
{
    public class ExOzProductResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string UrlSegment { get; set; }
        public string Summary { get; set; }
        public int OperatorId { get; set; }
        public string OperatorName { get; set; }
        public string OperatorPublicName { get; set; }
        public string OperatorUrlSegment { get; set; }
        public string CanonicalRegionUrlSegment { get; set; }
        public bool BookingRequired { get; set; }
        public string HandlerKey { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string MoreInfo { get; set; }
        public string Tips { get; set; }
        public List<string> Highlights { get; set; }
        public List<string> Images { get; set; }
        public List<ExOzSessionResponse> ProductSessions { get; set; }
        public List<Geolocation> Geolocations { get; set; }
        public string Timestamp { get; set; }
        public string Timezone { get; set; }
        public string HelpCode { get; set; }
    }

    public class ProductList
    {
        public List<ExOzProductResponse> Products { get; set; }
    }

    public class ProductImage
    {
        public int ImageId { get; set; }
        public int EventId { get; set; }
        public int ProductId { get; set; }
        public string ImageURL { get; set; }
    }
}