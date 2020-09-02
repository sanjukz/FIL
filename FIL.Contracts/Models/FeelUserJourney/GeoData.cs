using System.Collections.Generic;

namespace FIL.Contracts.Models
{
    public class GeoData
    {
        public SectionDetail SectionDetails { get; set; }
        public List<GeoLocation> Cities { get; set; }
        public List<GeoLocation> States { get; set; }
        public List<GeoLocation> Countries { get; set; }
        public string Url { get; set; }
    }
}