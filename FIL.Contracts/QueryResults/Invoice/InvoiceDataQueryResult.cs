using FIL.Contracts.Models.TMS.Invoice;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.Invoice
{
    public class InvoiceDataQueryResult
    {
        public List<InvoiceDetailModel> InvoiceDetailModels { get; set; }
    }
}