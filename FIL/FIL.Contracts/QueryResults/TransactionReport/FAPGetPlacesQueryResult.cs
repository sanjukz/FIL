using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.TransactionReport
{
    public class FAPGetPlacesQueryResult
    {
        public List<FIL.Contracts.Models.TransactionReport.FAPAllPlacesResponseModel> Places { get; set; }
    }
}