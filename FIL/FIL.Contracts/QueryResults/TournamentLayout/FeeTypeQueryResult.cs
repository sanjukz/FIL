using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.TournamentLayout
{
    public class FeeTypeQueryResult
    {
        public List<FeeType> FeeType { get; set; }
        public List<Channel> Channels { get; set; }
        public List<Models.ValueType> ValueType { get; set; }
    }
}