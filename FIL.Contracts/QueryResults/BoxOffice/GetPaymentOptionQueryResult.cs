using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.BoxOffice
{
    public class GetPaymentOptionQueryResult
    {
        public List<ZsuitePaymentOption> PaymentOptions { get; set; }
    }
}