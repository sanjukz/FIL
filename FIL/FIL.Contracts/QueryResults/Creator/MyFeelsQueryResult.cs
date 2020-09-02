using System.Collections.Generic;

namespace FIL.Contracts.QueryResults
{
    public class MyFeelsQueryResult
    {
        public List<FIL.Contracts.Models.Creator.MyFeel> MyFeels { get; set; }
        public bool Success { get; set; }
    }
}