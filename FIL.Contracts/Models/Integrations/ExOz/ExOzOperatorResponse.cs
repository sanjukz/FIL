using System;
using System.Collections.Generic;
using static FIL.Contracts.Models.Integrations.ExOz.ExOzGeolocationResponse;

namespace FIL.Contracts.Models.Integrations.ExOz
{
    public class ExOzOperatorResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PublicName { get; set; }
        public string UrlSegment { get; set; }
        public string CanonicalRegionUrlSegment { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string Tips { get; set; }
        public string Phone { get; set; }
        public List<Geolocation> Geolocations { get; set; }
        public int Quantity { get; set; }
        public List<ExOzProductResponse> Products { get; set; }
        public string Timestamp { get; set; }
        public string[] Images { get; set; }
        public int RegionId { get; set; }

        public long EventId { get; set; }
        public int VenueId { get; set; }

        public string RegionUrlSegment { get; set; }

        //Obsolete fields
        public double FromPrice { get; set; }

        public double RetailPrice { get; set; }
        public string Levy { get; set; }
        public string Rating { get; set; }
        public string Address { get; set; }
        public List<string> CategoryIds { get; set; }

        public Guid ModifiedBy { get; set; }

        public ExOzOperatorResponse()
        {
            this.Id = -1;
            this.EventId = 0;
            this.VenueId = 0;
            this.FromPrice = 0;
            this.RetailPrice = 0;
            this.Levy = string.Empty;
            this.Rating = string.Empty;
            this.Tips = string.Empty;
            this.Title = string.Empty;
            this.Description = string.Empty;
            this.Address = string.Empty;
            this.Phone = string.Empty;
        }
    }

    public class OperatorList
    {
        public List<ExOzOperatorResponse> operators { get; set; }
    }
}