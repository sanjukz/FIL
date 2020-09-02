using System.Collections.Generic;

namespace FIL.Contracts.Models.ValueRetail
{
    public class Desc
    {
        public string name { get; set; }
        public string description { get; set; }
    }

    public class VillageDescription
    {
        public IList<Desc> desc { get; set; }
    }
}