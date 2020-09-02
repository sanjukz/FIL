using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.Invoice
{
    public class InvoiceInformationDataQueryResult
    {
        public List<FIL.Contracts.Models.Event> Events { get; set; }
        public List<FIL.Contracts.Models.CurrencyType> Currencies { get; set; }
        public List<FIL.Contracts.Models.Sponsor> Sponsors { get; set; }
    }
}