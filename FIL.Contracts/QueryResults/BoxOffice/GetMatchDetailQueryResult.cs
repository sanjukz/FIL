using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.BoxOffice
{
    public class GetMatchDetailQueryResult
    {
        public List<GetMatchDetailContainer> GetMatchDetailContainer { get; set; }
    }
}