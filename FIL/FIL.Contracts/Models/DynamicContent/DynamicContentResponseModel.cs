using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Models.DynamicContent
{
    public class DynamicContentResponseModel
    {
        public string PlaceName { get; set; }
        public string PlaceDescription { get; set; }
        public string VenueName { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
        public string SubCategoryName { get; set; }
        public string SubCategoryUrl { get; set; }
        public string ParentCategoryName { get; set; }
        public string ParentCategoryUrl { get; set; }
        public string GenericLocationName { get; set; }
        public FIL.Contracts.Enums.MasterEventType MasterEventTypeId { get; set; }
        public Guid AltId { get; set; }
        public List<ProductMarkup> ProductMarkupModelList { get; set; }
    }

    public class ProductMarkup
    {
        public string @type { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public DateTime availabilityEnds { get; set; }
        public string priceCurrency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? lowPrice { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? highPrice { get; set; }

        public DateTime availabilityStarts { get; set; }
        public DateTime validFrom { get; set; }
        public string availability { get; set; }
        public decimal price { get; set; }
    }
}