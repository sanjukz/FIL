using System.Collections.Generic;

namespace FIL.Contracts.Models.Integrations.ExOz
{
    public class ExOzStateResponse
    {
        public int Id { get; set; }
        public int StateId { get; set; }
        public string Name { get; set; }
        public string UrlSegment { get; set; }
        public int CountryId { get; set; }
        public int StateMapId { get; set; }
        public bool IsEnabled { get; set; }
        public string Country { get; set; }
    }

    public class StateResponseList
    {
        public List<ExOzStateResponse> states { get; set; }
    }
}