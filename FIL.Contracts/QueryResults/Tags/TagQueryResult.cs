using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.Tags
{
    public class TagQueryResult
    {
        public IEnumerable<FIL.Contracts.DataModels.Tags> Tags { get; set; }
    }
}