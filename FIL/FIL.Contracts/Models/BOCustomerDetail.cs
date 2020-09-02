using System;

namespace FIL.Contracts.Models
{
    public class BOCustomerDetail
    {
        public long TransactionId { get; set; }
        public string ModeofPayment { get; set; }
        public string BankName { get; set; }
        public string ChequeNumber { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string ZipCode { get; set; }
    }
}