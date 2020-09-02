using System.Collections.Generic;

namespace FIL.Contracts.Models.Integrations.Kidzania
{
    public class VisitorTypeResponse
    {
        public List<VisitorType> VisitorTypes { get; set; }
    }

    public class VisitorType
    {
        public long VisitorTypeId { get; set; }
        public string VisitorTypeDesc { get; set; }
        public long VisitorMinAge { get; set; }
        public long VisitorMaxAge { get; set; }
        public long VisitorTypeAliasId { get; set; }
        public long PriceId { get; set; }
        public double Price { get; set; }
    }
}