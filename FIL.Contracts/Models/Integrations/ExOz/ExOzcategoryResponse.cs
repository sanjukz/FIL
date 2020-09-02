using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Contracts.Models.Integrations.ExOz
{
    public class ExOzCategoryResponse
    {
        public int id { get; set; }
        public string name { get; set; }
        public string urlSegment { get; set; }
        public int catId { get; set; }
        public int regionId { get; set; }
        public string regionUrl { get; set; }
        public int quantity { get; set; }
        public int offset { get; set; }
        public string timestamp { get; set; }
        public List<ExOzOperator> operators { get; set; }
    }
}