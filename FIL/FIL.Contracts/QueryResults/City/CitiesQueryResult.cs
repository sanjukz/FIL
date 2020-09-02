using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults
{
    public class CitiesQueryResult
    {
        public List<City> Cities { get; set; }
        public bool Success { get; set; }
    }
}