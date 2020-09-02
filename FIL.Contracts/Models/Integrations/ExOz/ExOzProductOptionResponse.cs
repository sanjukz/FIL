using System.Collections.Generic;

namespace FIL.Contracts.Models.Integrations.ExOz
{
    public class ExOzProductOptionResponse
    {
        public long Id { get; set; }
        public long SessionId { get; set; }
        public string SessionName { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string RetailPrice { get; set; }
        public int MaxQty { get; set; }
        public int MinQty { get; set; }
        public int DefaultQty { get; set; }
        public int Multiple { get; set; }
        public int Weight { get; set; }
        public bool IsFromPrice { get; set; }
    }

    public class ProductOptionList
    {
        public List<ExOzProductOptionResponse> ProductOptions { get; set; }
    }
}