using System.Collections.Generic;

namespace FIL.Contracts.Models.Integrations.ExOz
{
    public class ExOzRegionResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UrlSegment { get; set; }
        public int StateId { get; set; }
        public int Quantity { get; set; }
        public int Offset { get; set; }
        public string Timestamp { get; set; }
        public List<ExOzCategoryResponse> Categories { get; set; }
        public List<ExOzOperatorResponse> Operators { get; set; }
        public string StateUrlSegment { get; set; }
    }

    public class ExOzRegionList
    {
        public List<ExOzRegionResponse> regions { get; set; }
    }
}